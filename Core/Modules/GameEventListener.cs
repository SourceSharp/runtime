using SourceSharp.Core.Interfaces;
using SourceSharp.Core.Models;
using SourceSharp.Core.Utils;
using SourceSharp.Sdk;
using SourceSharp.Sdk.Attributes;
using SourceSharp.Sdk.Enums;
using SourceSharp.Sdk.Models;
using SourceSharp.Sdk.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SourceSharp.Core.Modules;

internal sealed class GameEventListener : IGameEventListener
{
    private class GameEventListenerInfo : CHookCallback<Func<GameEvent, ActionResponse<bool>>>
    {
        public GameEventHookType HookType { get; }

        internal GameEventListenerInfo(CPlugin plugin, GameEventHookType type, MethodInfo method) : base(plugin, method)
        {
            HookType = type;
        }
    }

    private readonly Dictionary<string, List<GameEventListenerInfo>> _listener;
    private readonly ISourceSharpBase _sourceSharp;

    public GameEventListener(ISourceSharpBase sourceSharp)
    {
        _sourceSharp = sourceSharp;
        _listener = new();
    }

    public void Initialize()
    {
        // Do nothing
    }

    public void Shutdown()
    {
        _listener.Clear();
    }

    public void OnPluginLoad(CPlugin plugin)
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

            hook.CheckReturnAndParameters(typeof(void), new[] { typeof(GameEvent) });

            if (!_listener.ContainsKey(ev.Name))
            {
                _listener.Add(ev.Name, new());
            }

            _listener[ev.Name].Add(new(plugin, ev.Type, hook));
        }
    }

    public void OnPluginUnload(CPlugin plugin)
    {
        foreach (var (eventName, hooks) in _listener.Where(x => x.Value.Any(v => v.Plugin == plugin)))
        {
            for (var i = 0; i < hooks.Count; i++)
            {
                if (hooks[i].Plugin.Equals(plugin))
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

    public bool OnFireEvent(GameEvent @event)
    {
        if (!_listener.TryGetValue(@event.Name, out var listeners))
        {
            return false;
        }

        int code = 0;
        bool block = false;

        foreach (var listener in listeners.Where(x => x.HookType is GameEventHookType.Pre))
        {
            var response = listener.Callback.Invoke(@event);
            if (response.Code > code)
            {
                code = response.Code;
                block = response.Response;
            }
        }

        foreach (var listener in listeners.Where(x => x.HookType is GameEventHookType.PostNoCopy))
        {
            // no-copy 为不包含 event数据
        }

        foreach (var listener in listeners.Where(x => x.HookType is GameEventHookType.Post))
        {
            // TODO DeepCopy
            var dummy = @event;
            listener.Callback.Invoke(dummy);
        }

        return block;
    }

    /*
     *  IRuntime
     */
    public string GetInterfaceName() => SharedDefines.GameEventListenerInterfaceName;
    public uint GetInterfaceVersion() => SharedDefines.GameEventListenerInterfaceVersion;
}
