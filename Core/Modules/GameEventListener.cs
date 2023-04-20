using SourceSharp.Core.Interfaces;
using SourceSharp.Core.Models;
using SourceSharp.Sdk.Attributes;
using SourceSharp.Sdk.Interfaces;
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
    private readonly ICore _core;

    public GameEventListener(ICore core)
    {
        _core = core;
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

    public void Shutdown()
    {

    }
}
