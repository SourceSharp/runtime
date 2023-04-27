using SourceSharp.Core.Interfaces;
using SourceSharp.Core.Models;
using SourceSharp.Sdk;
using SourceSharp.Sdk.Attributes;
using SourceSharp.Sdk.Enums;
using SourceSharp.Sdk.Models;
using SourceSharp.Sdk.Structs;
using System;
using System.Linq;

namespace SourceSharp.Core.Modules;

internal sealed class GameEventListener : IGameEventListener
{
    private readonly struct EventListenerInfo
    {
        public GameEventHookType HookType { get; } = GameEventHookType.Post;
        public int Priority { get; } = 0;

        public EventListenerInfo(GameEventHookType hookType, int priority)
        {
            HookType = hookType;
            Priority = priority;
        }
    }

    private readonly CKeyHook<string,
        EventListenerInfo,
        GameEventAttribute,
        bool,
        Func<GameEvent, ActionResponse<bool>>> _hooks;

    public GameEventListener()
    {
        _hooks = new();
    }

    public void Initialize()
    {
        // Do nothing
    }

    public void Shutdown()
    {
        _hooks.Shutdown();
    }

    public void OnPluginLoad(CPlugin plugin)
    {
        _hooks.ScanPlugin(plugin,
            attribute => new(attribute.Type, attribute.Priority),
            s => Bridges.Event.RegGameEventHook(s));
    }

    public void OnPluginUnload(CPlugin plugin)
        => _hooks.RemovePlugin(plugin);

    public bool OnEventFire(CGameEvent @event)
        => _hooks.OnCall(@event.Name, false, hooks =>
        {
            var code = 0;
            var block = false;

            foreach (var listener in hooks
                         .Where(x => x.Info.HookType is GameEventHookType.Pre)
                         .OrderByDescending(x => x.Info.Priority))
            {
                var response = listener.Callback.Invoke(@event);
                if (response.Code > code)
                {
                    code = response.Code;
                    block = response.Response;
                }
            }

            return block;
        });

    public void OnEventFired(CGameEvent @event)
        => _hooks.OnCall(@event.Name, false, hooks =>
        {
            foreach (var listener in hooks
                         .Where(x => x.Info.HookType is GameEventHookType.Post)
                         .OrderByDescending(x => x.Info.Priority))
            {
                listener.Callback.Invoke(@event);
            }

            return true;
        });

    /*
     *  IRuntime
     */
    public string GetInterfaceName() => SharedDefines.GameEventListenerInterfaceName;
    public uint GetInterfaceVersion() => SharedDefines.GameEventListenerInterfaceVersion;
}
