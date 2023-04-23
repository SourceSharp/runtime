using SourceSharp.Core.Interfaces;
using SourceSharp.Core.Models;
using SourceSharp.Sdk;
using SourceSharp.Sdk.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceSharp.Core.Modules;

internal class GameEventListener : IGameEventListener
{
    private class PluginGameEvent
    {
        public required SourceSharpPlugin Plugin { get; set; }
        public required Action<GameEvent> Callback { get; set; }
    }

    private readonly Dictionary<string, List<PluginGameEvent>> _listener;

    public GameEventListener()
    {
        _listener = new();
    }

    public void Initialize(List<SourceSharpPlugin> plugins)
    {
        plugins.ForEach(plugin =>
        {
            var hooks = plugin.Instance.GetType().GetMethods()
                .Where(m => Attribute.GetCustomAttributes(m, typeof(GameEventAttribute)).Any())
                .ToList();

            if (!hooks.Any())
            {
                return;
            }

            foreach (var hook in hooks)
            {
                if (Attribute.GetCustomAttribute(hook, typeof(GameEventAttribute)) is not GameEventAttribute ev)
                {
                    continue;
                }

                if (!_listener.ContainsKey(ev.Name))
                {
                    _listener.Add(ev.Name, new());
                }

                _listener[ev.Name].Add(new()
                {
                    Plugin = plugin,
                    Callback = (Action<GameEvent>)Delegate.CreateDelegate(typeof(Action<GameEvent>), hook)
                });
            }
        });
    }

    public void OnPluginUnload(SourceSharpPlugin plugin)
    {
        foreach (var (eventName, hooks) in _listener.Where(x => x.Value.Any(v => v.Plugin == plugin)))
        {
            for (var i = 0; i < hooks.Count; i++)
            {
                if (hooks[i].Plugin == plugin)
                {
                    hooks.RemoveAt(i);
                    i--;
                }
            }

            if (!hooks.Any())
            {
                _listener.Remove(eventName);
                break;
            }
        }
    }

    public void Shutdown()
    {
        throw new NotImplementedException();
    }

    /*
     *  IRuntime
     */
    public string GetInterfaceName() => SharedDefines.GameEventListenerInterfaceName;
    public uint GetInterfaceVersion() => SharedDefines.GameEventListenerInterfaceVersion;
}
