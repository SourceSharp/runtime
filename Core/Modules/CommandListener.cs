using SourceSharp.Core.Interfaces;
using SourceSharp.Core.Models;
using SourceSharp.Sdk;
using SourceSharp.Sdk.Attributes;
using SourceSharp.Sdk.Enums;
using SourceSharp.Sdk.Interfaces;
using SourceSharp.Sdk.Models;
using System;

namespace SourceSharp.Core.Modules;

internal sealed class CommandListener : ICommandListener
{
    private readonly struct ConsoleCommandInfo
    {
        public string Command { get; }
        public string Description { get; }
        public ConVarFlags Flags { get; }
        public AdminFlags AccessFlags { get; }

        public ConsoleCommandInfo(string command, string description, ConVarFlags flags, AdminFlags accessFlags)
        {
            Command = command;
            Description = description;
            Flags = flags;
            AccessFlags = accessFlags;
        }
    }

    private readonly CKeyHook<string,
        ConsoleCommandInfo,
        ServerConsoleCommandAttribute,
        bool,
        Action<ConsoleCommand>> _server;

    private readonly CKeyHook<string,
        ConsoleCommandInfo,
        ClientConsoleCommandAttribute,
        bool,
        Action<ConsoleCommand, GamePlayer?>> _client;

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
        _server.Shutdown();
        _client.Shutdown();
    }

    public void OnPluginLoad(CPlugin plugin)
    {
        _client.ScanPlugin(plugin,
            attr => new ConsoleCommandInfo(attr.Key, attr.Description, attr.Flags, AdminFlags.None),
            Bridges.ConCommand.RegClientCommand);

        _server.ScanPlugin(plugin,
            attr => new ConsoleCommandInfo(attr.Key, attr.Description, attr.Flags, AdminFlags.None),
            Bridges.ConCommand.RegServerCommand);
    }

    public void OnPluginUnload(CPlugin plugin)
    {
        _client.RemovePlugin(plugin);
        _server.RemovePlugin(plugin);
    }

    public bool OnClientConsoleCommand(ConsoleCommand command, GamePlayer? player)
        => _client.OnCall(command.Command, false, hooks =>
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

            foreach (var hook in hooks)
            {
                if (accessFlags.HasFlag(hook.Info.AccessFlags))
                {
                    hook.Callback.Invoke(command, player);
                }
            }

            return true;
        });

    public bool OnServerConsoleCommand(ConsoleCommand command)
        => _server.OnCall(command.Command, false, hooks =>
        {
            foreach (var hook in hooks)
            {
                hook.Callback.Invoke(command);
            }
            return true;
        });

    /*
     *  IRuntime
     */
    public string GetInterfaceName() => SharedDefines.CommandListenerInterfaceName;
    public uint GetInterfaceVersion() => SharedDefines.CommandListenerInterfaceVersion;
}
