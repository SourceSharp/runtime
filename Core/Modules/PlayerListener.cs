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

namespace SourceSharp.Core.Modules;

internal class PlayerListener : IPlayerListener
{
    private class PlayerEvent
    {
        public required CPlugin Plugin { get; set; }
        public required Action<GamePlayer> Callback { get; set; }
    }

    private class PlayerHook
    {
        public required CPlugin Plugin { get; set; }
        public required Func<ulong, IPEndPoint, string, string, ActionResponse<string>> Callback { get; set; }
    }

    private readonly List<PlayerEvent> _events;
    private readonly List<PlayerHook> _hooks;
    private readonly ISourceSharpBase _sourceSharp;

    public PlayerListener(ISourceSharpBase sourceSharp)
    {
        _sourceSharp = sourceSharp;

        _events = new();
        _hooks = new();
    }

    public void Initialize()
    {
        throw new NotImplementedException();
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
                hook.CheckReturnAndParameters(typeof(bool),
                    new[] { typeof(ulong), typeof(string), typeof(string), typeof(string), typeof(string) });

                _hooks.Add(new()
                {
                    Plugin = plugin,
                    Callback = (Func<ulong, IPEndPoint, string, string, ActionResponse<string>>)Delegate.CreateDelegate(typeof(Func<ulong, IPEndPoint, string, string, ActionResponse<string>>), hook),
                });
            }
            else
            {
                hook.CheckReturnAndParameters(typeof(void), new[] { typeof(CGamePlayer) });

                _events.Add(new()
                {
                    Plugin = plugin,
                    Callback = (Action<GamePlayer>)Delegate.CreateDelegate(typeof(Action<GamePlayer>), hook)
                });
            }
        }
    }

    public void OnPluginUnload(CPlugin plugin)
    {
        _hooks.RemoveAll(x => x.Plugin.Equals(plugin));
        _events.RemoveAll(x => x.Plugin.Equals(plugin));
    }

    public void Shutdown()
    {
        throw new NotImplementedException();
    }

    /*
     * Listener
     */
    public string? OnConnectHook(ulong steamId, string ip, ushort port, string name, string password)
    {
        string? returnValue = null;
        var code = 0;

        var ep = new IPEndPoint(IPAddress.Parse(ip), port);

        foreach (var hook in _hooks)
        {
            var response = hook.Callback.Invoke(steamId, ep, name, password);
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
