using McMaster.NETCore.Plugins;
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
using System.Reflection;

namespace SourceSharp.Core;

internal sealed class PluginManager : IPluginManager
{
    private readonly ISourceSharpBase _sourceSharp;
    private readonly IShareSystemBase _shareSystem;

    private readonly List<CPlugin> _plugins;
    private readonly List<IListenerBase> _listeners;

    public PluginManager(ISourceSharpBase sourceSharp, IShareSystemBase shareSystem)
    {
        _sourceSharp = sourceSharp;
        _shareSystem = shareSystem;

        _plugins = new();
        _listeners = new();
    }

    public void Initialize()
    public void Initialize(IServiceProvider services)
    {
        var listeners = services.GetAllServices<IListenerBase>();
        _listeners.AddRange(listeners);

        var sourceSharpRoot = Path.Combine(_sourceSharp.GetRootPath(), _sourceSharp.GetGamePath(), "addons", "sourcesharp");

        foreach (var path in Directory.GetDirectories(Path.Combine(sourceSharpRoot, "plugins"), "*", SearchOption.TopDirectoryOnly))
        {
            CPlugin? plugin = null;
            var name = Path.GetFileName(path);
            try
            {
                var file = Path.Combine(path, $"{name}.dll");

                if (!File.Exists(file))
                {
                    throw new DllNotFoundException($"Plugin dll not found.");
                }

                var absolutePath = Path.GetFullPath(file);

                var loader = PluginLoader.CreateFromAssemblyFile(absolutePath, config =>
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

                var pa = Attribute.GetCustomAttribute(tPlugin, typeof(PluginAttribute)) as PluginAttribute ??
                         throw new BadImageFormatException("Plugin metadata not found");

                plugin = new CPlugin(file, loader, instance, pa);
                _plugins.Add(plugin);

                tPlugin.SetProtectedReadOnlyField("_sourceSharp", instance, _sourceSharp);
                tPlugin.SetProtectedReadOnlyField("_shareSystem", instance, _shareSystem);

                var frameHooks = tPlugin.GetMethods()
                    .Where(m => Attribute.GetCustomAttributes(m, typeof(GameFrameAttribute)).Any())
                    .ToList();

                if (frameHooks.Any())
                {
                    if (frameHooks.Count > 1)
                    {
                        throw new BadImageFormatException("Multiple GameFrameHook found.");
                    }

                    var frameHook = frameHooks.Single();
                    frameHook.CheckReturnAndParameters(typeof(void), new[] { typeof(bool) });
                    plugin.AddGameHook(frameHook);
                }

                foreach (var listener in _listeners)
                {
                    listener.OnPluginLoad(plugin);
                }
            }
            catch (Exception e)
            {
                plugin?.UpdateStatus(PluginStatus.Failed);
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
                plugin.UpdateStatus(PluginStatus.Error);

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
            UnloadPlugin(plugin, false);
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

            plugin.UpdateStatus(PluginStatus.Running);
            _sourceSharp.PrintLine($"Plugin <{plugin.Name}> loaded.");
        }
        catch (Exception e)
        {
            plugin.UpdateStatus(PluginStatus.Failed);

            if (e is not InvalidOperationException)
            {
                _sourceSharp.LogMessage($"Failed to load plugin <{plugin.Name}>: {e.Message}{Environment.NewLine}{e.StackTrace}");
            }
        }
    }

    private void UnloadPlugin(CPlugin plugin, bool remove)
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

            foreach (var listener in _listeners)
            {
                listener.OnPluginUnload(plugin);
            }

            plugin.Instance.OnShutdown();
            plugin.Loader.Dispose();
            plugin.UpdateStatus(PluginStatus.None);

            if (remove)
            {
                _plugins.Remove(plugin);
            }
        }
        catch (Exception e)
        {
            _sourceSharp.LogError($"Error during shutting down on <{plugin.Name}>: {e.Message}{Environment.NewLine}{e.StackTrace}");
        }
    }
}