using McMaster.NETCore.Plugins;
using SourceSharp.Core.Configurations;
using SourceSharp.Core.Interfaces;
using SourceSharp.Core.Models;
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

    private readonly List<SourceSharpPlugin> _plugins;

    public PluginManager(CoreConfig config, ISourceSharpBase sourceSharp, IShareSystemBase shareSystem)
    {
        _config = config;
        _sourceSharp = sourceSharp;
        _shareSystem = shareSystem;

        _plugins = new();
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

                // TODO MaxPlayers correct value
                var mp = tPlugin.GetProperties().Single(x => x.Name == "MaxPlayers");
                mp.SetValue(instance, 64);

                var pa = Attribute.GetCustomAttribute(tPlugin, typeof(PluginAttribute)) as PluginAttribute ??
                         throw new BadImageFormatException("Plugin metadata not found");

                _plugins.Add(new()
                {
                    Path = file,
                    Instance = instance,
                    Status = PluginStatus.Checked,
                    Loader = loader,

                    Name = pa.Name,
                    Author = pa.Author,
                    Version = pa.Version,
                    Url = pa.Url,
                    Description = pa.Description
                });

                _sourceSharp.PrintLine($"Plugin <{name}> checked.");
            }
            catch (Exception e)
            {
                loader?.Dispose();
                _sourceSharp.LogMessage($"Failed to load plugin <{name}>: {e.Message}{Environment.NewLine}{e.StackTrace}");
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

    private void LoadPlugins()
    {
        foreach (var plugin in _plugins)
        {
            LoadPlugin(plugin);
        }
    }

    private void LoadPlugin(SourceSharpPlugin plugin)
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

    private void UnloadPlugin(SourceSharpPlugin plugin)
    {
        try
        {
            _shareSystem.CheckUnloadPluginInterfaces(plugin.Instance);
            plugin.Instance.OnShutdown();
            plugin.Loader.Dispose();
            plugin.Status = PluginStatus.None;
        }
        catch (Exception e)
        {
            _sourceSharp.LogError($"Error during shutting down on <{plugin.Name}>: {e.Message}{Environment.NewLine}{e.StackTrace}");
        }
    }

    private void InspectPlugin(SourceSharpPlugin plugin)
    {
        throw new NotImplementedException();
    }
}
