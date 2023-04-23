using McMaster.NETCore.Plugins;
using SourceSharp.Core.Configurations;
using SourceSharp.Core.Interfaces;
using SourceSharp.Core.Models;
using SourceSharp.Core.Utils;
using SourceSharp.Sdk.Attributes;
using SourceSharp.Sdk.Enums;
using SourceSharp.Sdk.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SourceSharp.Core;

internal class PluginManager : IPluginManager
{
    private readonly CoreConfig _config;
    private readonly ISourceSharpBase _sourceSharp;
    private readonly IShareSystemBase _shareSystem;

    private readonly List<CPlugin> _plugins;

    // TODO MaxPlayers correct value
    private readonly uint _maxPlayers;

    public PluginManager(CoreConfig config, ISourceSharpBase sourceSharp, IShareSystemBase shareSystem)
    {
        _config = config;
        _sourceSharp = sourceSharp;
        _shareSystem = shareSystem;

        _plugins = new();

        _maxPlayers = 64;
    }

    public void Initialize()
    {
        foreach (var path in Directory.GetDirectories(Path.Combine("plugins"), "*", SearchOption.TopDirectoryOnly))
        {
            PluginLoader? loader = null;
            var name = Path.GetDirectoryName(path)!;
            try
            {
                var file = Path.Combine(path, $"{name}.dll");
                if (!File.Exists(file))
                {
                    throw new DllNotFoundException($"Plugin dll not found.");
                }

                loader = PluginLoader.CreateFromAssemblyFile(file, config =>
                {
                    config.PreferSharedTypes = true;
                    config.IsUnloadable = true;
                    config.LoadInMemory = true;
                });

                var tPlugin = loader.LoadDefaultAssembly().GetTypes()
                                  .SingleOrDefault(t => typeof(PluginBase).IsAssignableFrom(t) && !t.IsAbstract) ??
                              throw new FileLoadException("IPlugin is not implemented.");

                if (Activator.CreateInstance(tPlugin) is not PluginBase instance)
                {
                    throw new InvalidOperationException("Failed to create instance.");
                }

                var sourceSharp = tPlugin.GetProperties().Single(x => x.Name == "_sourceSharp");
                sourceSharp.SetValue(instance, _sourceSharp);

                var shareSystem = tPlugin.GetProperties().Single(x => x.Name == "_shareSystem");
                shareSystem.SetValue(instance, _shareSystem);

                var mp = tPlugin.GetProperties().Single(x => x.Name == "MaxPlayers");
                mp.SetValue(instance, _maxPlayers);

                var pa = Attribute.GetCustomAttribute(tPlugin, typeof(PluginAttribute)) as PluginAttribute ??
                         throw new BadImageFormatException("Plugin metadata not found");

                var fhc = tPlugin.GetMethods()
                    .Where(m => Attribute.GetCustomAttributes(m, typeof(GameFrameAttribute)).Any())
                    .ToList();

                if (fhc.Count > 1)
                {
                    throw new BadImageFormatException("Multiple GameFrameHook found.");
                }

                var fh = fhc.Single();
                fh.CheckReturnAndParameters(typeof(void), new[] { typeof(bool) });
                var gh = (Action<bool>)Delegate.CreateDelegate(typeof(Action<bool>), fhc.Single());

                _plugins.Add(new(file, loader, instance, gh, pa));
                _sourceSharp.PrintLine($"Plugin <{name}> checked.");
            }
            catch (Exception e)
            {
                loader?.Dispose();
                _sourceSharp.LogError($"Failed to load plugin <{name}>: {e.Message}{Environment.NewLine}{e.StackTrace}");
            }
        }

        LoadPlugins();
    }

    public void Signal()
    {
        foreach (var plugin in _plugins.Where(x => x.Status == PluginStatus.Running))
        {
            try
            {
                if (plugin.Instance.QueryRunning())
                {
                    throw new InvalidOperationException();
                }
            }
            catch (Exception e)
            {
                plugin.Status = PluginStatus.Error;

                if (e is not InvalidOperationException)
                {
                    _sourceSharp.LogMessage($"Failed to load plugin <{plugin.Name}>: {e.Message}{Environment.NewLine}{e.StackTrace}");
                }
            }
        }
    }

    public void Shutdown()
    {
        foreach (var plugin in _plugins)
        {
            UnloadPlugin(plugin);
        }

        _plugins.Clear();
    }

    public void OnGameFrame(bool simulating)
    {
        foreach (var plugin in _plugins.Where(x => x.Status is PluginStatus.Running && x.FrameHook is not null))
        {
            plugin.FrameHook?.Invoke(simulating);
        }
    }

    private void LoadPlugins()
    {
        foreach (var plugin in _plugins.Where(x => x.Status is PluginStatus.Checked))
        {
            LoadPlugin(plugin);
        }
    }

    private void LoadPlugin(CPlugin plugin)
    {
        try
        {
            if (!plugin.Instance.OnLoad())
            {
                throw new InvalidOperationException();
            }

            plugin.Status = PluginStatus.Running;
            _sourceSharp.PrintLine($"Plugin <{plugin.Name}> loaded.");
        }
        catch (Exception e)
        {
            plugin.Status = PluginStatus.Failed;

            if (e is not InvalidOperationException)
            {
                _sourceSharp.LogMessage($"Failed to load plugin <{plugin.Name}>: {e.Message}{Environment.NewLine}{e.StackTrace}");
            }
        }
    }

    private void UnloadPlugin(CPlugin plugin)
    {
        try
        {
            var interfaces = _shareSystem.CheckUnloadPluginInterfaces(plugin.Instance);
            if (interfaces.Any())
            {
                foreach (var @interface in interfaces)
                {
                    foreach (var p in _plugins.Where(x => !x.Equals(plugin)))
                    {
                        p.Instance.NotifyInterfaceDrop(@interface);
                    }
                }
            }
            plugin.Instance.OnShutdown();
            plugin.Loader.Dispose();
            plugin.Status = PluginStatus.None;
        }
        catch (Exception e)
        {
            _sourceSharp.LogError($"Error during shutting down on <{plugin.Name}>: {e.Message}{Environment.NewLine}{e.StackTrace}");
        }
    }

    private void InspectPlugin(CPlugin plugin)
    {
        throw new NotImplementedException();
    }
}
