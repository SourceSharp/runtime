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
using System.Net;
using System.Reflection;

namespace SourceSharp.Core.Modules;

internal sealed class PlayerListener : IPlayerListener
{
    private class PlayerEvent : CHookCallback<Action<GamePlayer>>
    {
        internal PlayerEvent(CPlugin plugin, MethodInfo method) : base(plugin, method) { }
    }

    private class PlayerHook : CHookCallback<Func<ulong, IPEndPoint, string, string, ActionResponse<string>>>
    {
        internal PlayerHook(CPlugin plugin, MethodInfo method) : base(plugin, method) { }
    }

    private readonly List<PlayerEvent> _events;
    private readonly List<PlayerHook> _hooks;

    public PlayerListener()
    {
        _events = new();
        _hooks = new();
    }

    public void Initialize()
    {
        // Do nothing...
    }

    public void Shutdown()
    {
        _events.Clear();
        _hooks.Clear();
    }

    public void OnPluginLoad(CPlugin plugin)
    {
        var hooks = plugin.Instance.GetType().GetMethods()
            .Where(m => Attribute.GetCustomAttributes(m, typeof(PlayerListenerAttribute)).Any())
            .ToList();

        if (!hooks.Any())
        {
            return;
        }

        foreach (var hook in hooks)
        {
            if (Attribute.GetCustomAttribute(hook, typeof(PlayerListenerAttribute)) is not PlayerListenerAttribute ev)
            {
                continue;
            }

            if (ev.Type is PlayerListenerType.ConnectHook)
            {
                hook.CheckReturnAndParameters(typeof(bool), new[] { typeof(ulong), typeof(string), typeof(string), typeof(string), typeof(string) });
                _hooks.Add(new(plugin, hook));
            }
            else
            {
                hook.CheckReturnAndParameters(typeof(void), new[] { typeof(CGamePlayer) });
                _events.Add(new(plugin, hook));
            }
        }
    }

    public void OnPluginUnload(CPlugin plugin)
    {
        _hooks.RemoveAll(x => x.Plugin.Equals(plugin));
        _events.RemoveAll(x => x.Plugin.Equals(plugin));
    }

    /*
     * Listener
     */
    public string? OnConnectHook(ulong steamId, IPEndPoint remoteEndPoint, string name, string password)
    {
        string? returnValue = null;
        var code = 0;

        foreach (var hook in _hooks)
        {
            var response = hook.Callback.Invoke(steamId, remoteEndPoint, name, password);
            if (response.Code > code)
            {
                returnValue = response.Response;
                code = response.Code;
            }
        }

        return returnValue;
    }

    public void OnConnected(CGamePlayer player)
    {
        throw new NotImplementedException();
    }

    public void OnAuthorized(CGamePlayer player)
    {
        throw new NotImplementedException();
    }

    public void OnPutInServer(CGamePlayer player)
    {
        throw new NotImplementedException();
    }

    public void OnPostAdminCheck(CGamePlayer player)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnecting(CGamePlayer player)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnected(CGamePlayer player)
    {
        throw new NotImplementedException();
    }

    /*
     *  IRuntime
     */
    public string GetInterfaceName() => SharedDefines.PlayerListenerInterfaceName;
    public uint GetInterfaceVersion() => SharedDefines.PlayerListenerInterfaceVersion;
}
