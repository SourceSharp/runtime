using SourceSharp.Core.Interfaces;
using SourceSharp.Core.Models;
using SourceSharp.Core.Structs;
using SourceSharp.Core.Utils;
using SourceSharp.Sdk;
using SourceSharp.Sdk.Attributes;
using SourceSharp.Sdk.Enums;
using SourceSharp.Sdk.Interfaces;
using SourceSharp.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SourceSharp.Core.Modules;

internal sealed class CommandListener : ICommandListener
{
    private abstract class ConsoleCommandEvent<T> where T : Delegate
    {
        public ConsoleCommandInfo Command { get; }
        public CPlugin Plugin { get; }
        public T Callback { get; }

        protected ConsoleCommandEvent(ConsoleCommandInfo command, CPlugin plugin, MethodInfo method)
        {
            Command = command;
            Plugin = plugin;
            Callback = method.CreateDelegate<T>(plugin.Instance);
        }
    }

    private class ServerConsoleEvent : ConsoleCommandEvent<Action<ConsoleCommand>>
    {
        public ServerConsoleEvent(ConsoleCommandInfo command, CPlugin plugin, MethodInfo method) : base(command, plugin, method) { }
    }

    private sealed class ClientConsoleEvent : ConsoleCommandEvent<Action<ConsoleCommand, GamePlayer?>>
    {
        public ClientConsoleEvent(ConsoleCommandInfo command, CPlugin plugin, MethodInfo method) : base(command, plugin, method) { }
    }

    private readonly Dictionary<string, List<ClientConsoleEvent>> _client;
    private readonly Dictionary<string, List<ServerConsoleEvent>> _server;

    private readonly IAdminManager _adminManager;

    public CommandListener(IAdminManager adminManager)
    {
        _client = new();
        _server = new();

        _adminManager = adminManager;
    }

    public void Initialize()
    {
        // do nothing
    }

    public void Shutdown()
    {
        _server.Clear();
        _client.Clear();
    }

    public void OnPluginLoad(CPlugin plugin)
    {
        var hooks = plugin.Instance.GetType().GetMethods()
            .Where(m => Attribute.GetCustomAttributes(m, typeof(ConsoleCommandBase)).Any())
            .ToList();

        if (!hooks.Any())
        {
            return;
        }

        foreach (var hook in hooks)
        {
            if (Attribute.GetCustomAttribute(hook, typeof(ConsoleCommandBase)) is not ConsoleCommandBase attr)
            {
                continue;
            }

            if (attr is ServerConsoleCommand sc)
            {
                hook.CheckReturnAndParameters(typeof(void), new[] { typeof(ConsoleCommand) });

                if (!_server.ContainsKey(attr.Command))
                {
                    _server.Add(sc.Command, new());

                    // TODO register to Engine
                }

                _server[sc.Command].Add(new(new ConsoleCommandInfo(sc.Command, sc.Description, sc.Flags, AdminFlags.None), plugin, hook));
            }
            else if (attr is ClientConsoleCommand cc)
            {
                hook.CheckReturnAndParameters(typeof(void), new[] { typeof(ConsoleCommand), typeof(GamePlayer) });

                if (!_client.ContainsKey(attr.Command))
                {
                    _client.Add(cc.Command, new());

                    // TODO register to Engine
                }

                _client[cc.Command].Add(new(new ConsoleCommandInfo(cc.Command, cc.Description, cc.Flags, cc.AccessFlags), plugin, hook));
            }
        }
    }

    public void OnPluginUnload(CPlugin plugin)
    {
        foreach (var (eventName, hooks) in _server.Where(x => x.Value.Any(v => v.Plugin == plugin)))
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
                _server.Remove(eventName);
                break;
            }
        }

        foreach (var (eventName, hooks) in _client.Where(x => x.Value.Any(v => v.Plugin == plugin)))
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
                _server.Remove(eventName);
                break;
            }
        }
    }

    public void OnClientConsoleCommand(ConsoleCommand command, GamePlayer? player)
    {
        // 这里与SourceMod不同
        // SM中注册命令的权限是以第一个注册的代码为准
        // 在SS中的实现为根据每个Attribute中的Flags
        // 来确定能否调用对应的Action

        var accessFlags = AdminFlags.None;

        AdminUser? admin;
        if (player is CGamePlayer { IsAuthorized: true } cp && (admin = _adminManager.FindAdminByIdentity(cp.SteamId)) is not null)
        {
            accessFlags = admin.Flags;
        }

        foreach (var (_, hook) in _client.Where(x => x.Key == command.Args[0]))
        {
            hook.ForEach(x =>
            {
                if (accessFlags.HasFlag(x.Command.AccessFlags))
                {
                    x.Callback.Invoke(command, player);
                }
            });
        }
    }

    public void OnServerConsoleCommand(ConsoleCommand command)
    {
        foreach (var (_, hook) in _server.Where(x => x.Key == command.Args[0]))
        {
            hook.ForEach(x => x.Callback.Invoke(command));
        }
    }

    /*
     *  IRuntime
     */
    public string GetInterfaceName() => SharedDefines.CommandListenerInterfaceName;
    public uint GetInterfaceVersion() => SharedDefines.CommandListenerInterfaceVersion;
}
