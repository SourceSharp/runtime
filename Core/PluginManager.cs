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
    private readonly ICore _core;
    private readonly IGameEventListener _gameEventListener;

    private readonly List<SourceSharpPlugin> _plugins;

    public PluginManager(CoreConfig config, ICore core, IGameEventListener gameEventListener)
    {
        _config = config;
        _core = core;
        _gameEventListener = gameEventListener;

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
                                  .SingleOrDefault(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsAbstract) ??
                              throw new FileLoadException("IPlugin is not implemented.");

                if (Activator.CreateInstance(tPlugin, _core) is not IPlugin instance)
                {
                    throw new InvalidOperationException("Failed to create instance.");
                }

                var pa = Attribute.GetCustomAttribute(tPlugin, typeof(PluginAttribute)) as PluginAttribute ??
                         throw new InvalidDataException("Plugin metadata not found");

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

                _core.PrintLine($"Plugin <{name}> checked.");
            }
            catch (Exception e)
            {
                if (loader is not null)
                {
                    loader.Dispose();
                }

                _core.LogMessage($"Failed to load plugin <{name}>: {e.Message}{Environment.NewLine}{e.StackTrace}");
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
                    _core.LogMessage($"Failed to load plugin <{plugin.Name}>: {e.Message}{Environment.NewLine}{e.StackTrace}");
                }
            }
        }
    }

    public void Shutdown()
    {
        _gameEventListener.Shutdown();

        foreach (var plugin in _plugins)
        {
            try
            {
                plugin.Instance.OnShutdown();
                plugin.Loader.Dispose();
                plugin.Status = PluginStatus.None;
            }
            catch (Exception e)
            {
                _core.LogError($"Error during shutting down on <{plugin.Name}>: {e.Message}{Environment.NewLine}{e.StackTrace}");
            }
        }

        _plugins.Clear();
    }

    private void LoadPlugins()
    {
        foreach (var plugin in _plugins)
        {
            try
            {
                if (!plugin.Instance.OnLoad())
                {
                    throw new InvalidOperationException();
                }

                plugin.Status = PluginStatus.Running;
                _core.PrintLine($"Plugin <{plugin.Name}> loaded.");
            }
            catch (Exception e)
            {
                plugin.Status = PluginStatus.Failed;

                if (e is not InvalidOperationException)
                {
                    _core.LogMessage($"Failed to load plugin <{plugin.Name}>: {e.Message}{Environment.NewLine}{e.StackTrace}");

                }
            }
        }

        _gameEventListener.Initialize(_plugins);
    }

    private void InspectPlugin(SourceSharpPlugin plugin)
    {

    }
}
