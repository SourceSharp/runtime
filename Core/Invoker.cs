using Microsoft.Extensions.DependencyInjection;
using SourceSharp.Core.Bridges;
using SourceSharp.Core.Interfaces;
using SourceSharp.Core.Models;
using SourceSharp.Sdk.Models;
using System;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace SourceSharp.Core;

internal static class Invoker
{
#nullable disable
    private static IPluginManager _pluginManager;
    private static ISourceSharpBase _sourceSharp;
    private static ICommandListener _commandListener;
    private static IPlayerManagerBase _playerManager;
    private static IPlayerListener _playerListener;
    private static IGameEventListener _gameEventListener;
    private static IConVarManager _conVarManager;
#nullable restore

    internal static void Initialize(IServiceProvider services)
    {
        _pluginManager = services.GetRequiredService<IPluginManager>();
        _sourceSharp = services.GetRequiredService<ISourceSharpBase>();
        _commandListener = services.GetRequiredService<ICommandListener>();
        _playerManager = services.GetRequiredService<IPlayerManagerBase>();
        _playerListener = services.GetRequiredService<IPlayerListener>();
        //_gameEventListener = services.GetRequiredService<IGameEventListener>();
        //_conVarManager = services.GetRequiredService<IConVarManager>();
    }

    /*
     * Bool 不是可导出/导入的类型!
     */

    [UnmanagedCallersOnly]
    public static void OnGameFrame(
        [DNNE.C99Type("uint8_t")] sbyte simulating,
        [DNNE.C99Type("int32_t")] int tickCount,
        [DNNE.C99Type("float")] float gameTime)
    {
        _sourceSharp.RunFrame();
        _pluginManager.OnGameFrame(simulating > 0);
    }

    #region Command Listener

    [UnmanagedCallersOnly]
    public static int ServerConsoleCommand(
        [DNNE.C99Type("const char*")] IntPtr pArgString,
        [DNNE.C99Type("const char**")] IntPtr pArgs,
        [DNNE.C99Type("int32_t")] int argc)
    {
        var command = ParseCommand(pArgString, pArgs, argc);
        if (command is null)
        {
            return 1;
        }
        _commandListener.OnServerConsoleCommand(command);
        return 0;
    }

    [UnmanagedCallersOnly]
    public static int ClientConsoleCommand(
        [DNNE.C99Type("const char*")] IntPtr pArgString,
        [DNNE.C99Type("const char**")] IntPtr pArgs,
        [DNNE.C99Type("int32_t")] int argc,
        [DNNE.C99Type("int32_t")] int clientIndex)
    {
        var command = ParseCommand(pArgString, pArgs, argc);
        if (command is null)
        {
            return 1;
        }

        GamePlayer? player = null;

        if (clientIndex > 0)
        {
            player = _playerManager.GetGamePlayer(clientIndex);
            if (player is null)
            {
                return 2;
            }
        }

        _commandListener.OnClientConsoleCommand(command, player);
        return 0;
    }

    private static ConsoleCommand? ParseCommand(IntPtr pArgString, IntPtr pArgs, int argc)
    {
        var argString = Marshal.PtrToStringAnsi(pArgString);

        if (argString is null)
        {
            return null;
        }

        var args = argc > 0 ? Enumerable.Range(0, argc - 1).Select(index =>
        {
            if (Marshal.PtrToStructure(IntPtr.Add(pArgs, index * sizeof(int)), typeof(IntPtr)) is IntPtr strPtr)
            {
                var result = Marshal.PtrToStringAnsi(strPtr);

                if (result is not null)
                {
                    return result;
                }
            }

            return string.Empty;
        }).ToArray() : new[] { argString };

        return new ConsoleCommand(argString, args, argc);
    }

    #endregion

    #region Event Listener

    [UnmanagedCallersOnly]
    public static int OnFireEvent([DNNE.C99Type("const void*")] IntPtr pEvent)
    {
        var @event = SSGameEvent.__CreateInstance(pEvent);
        return _gameEventListener.OnEventFire(new CGameEvent(@event, false)) ? 1 : 0;
    }

    [UnmanagedCallersOnly]
    public static void OnFiredEvent([DNNE.C99Type("const void*")] IntPtr pEvent)
    {
        var @event = SSGameEvent.__CreateInstance(pEvent);
        _gameEventListener.OnEventFired(new CGameEvent(@event, true));
    }

    #endregion

    #region Player Manager

    [UnmanagedCallersOnly]
    public static int OnClientConnected(
        [DNNE.C99Type("int32_t")] int clientIndex,
        [DNNE.C99Type("int32_t")] int userId,
        [DNNE.C99Type("uint64_t")] ulong steamId,
        [DNNE.C99Type("const char*")] IntPtr pName,
        [DNNE.C99Type("const char*")] IntPtr pINetAdr /* "114.114.114.114:12356" */)
    {
        var name = Marshal.PtrToStringAnsi(pName);
        var iNet = Marshal.PtrToStringAnsi(pINetAdr);
        if (name is null || iNet is null)
        {
            return 1;
        }

        if (!IPEndPoint.TryParse(iNet, out var ipEp))
        {
            return 2;
        }

        _playerManager.OnConnected(clientIndex, userId, steamId, name, ipEp);
        return 0;
    }

    [UnmanagedCallersOnly]
    public static void OnAuthorized(
        [DNNE.C99Type("int32_t")] int clientIndex,
        [DNNE.C99Type("uint64_t")] ulong steamId)
    {
        _playerManager.OnAuthorized(clientIndex, steamId);
    }

    [UnmanagedCallersOnly]
    public static void OnPutInServer(
        [DNNE.C99Type("int32_t")] int clientIndex,
        [DNNE.C99Type("uint8_t")] sbyte fakeClient,
        [DNNE.C99Type("uint8_t")] sbyte sourceTv,
        [DNNE.C99Type("uint8_t")] sbyte replay)
    {
        _playerManager.OnPutInServer(clientIndex, fakeClient > 0, sourceTv > 0, replay > 0);
    }

    [UnmanagedCallersOnly]
    public static void OnDisconnecting([DNNE.C99Type("int32_t")] int clientIndex)
    {
        _playerManager.OnDisconnecting(clientIndex);
    }

    [UnmanagedCallersOnly]
    public static void OnDisconnected([DNNE.C99Type("int32_t")] int clientIndex)
    {
        _playerManager.OnDisconnected(clientIndex);
    }

    [UnmanagedCallersOnly]
    public static void OnNameChanged(
        [DNNE.C99Type("int32_t")] int clientIndex,
        [DNNE.C99Type("const char*")] IntPtr pName)
    {
        var name = Marshal.PtrToStringAnsi(pName);
        if (name == null)
        {
            return;
        }
        _playerManager.UpdatePlayerName(clientIndex, name);
    }

    [UnmanagedCallersOnly]
    public static void OnNetChannelChanged([DNNE.C99Type("int32_t")] int clientIndex)
    {
        _playerManager.OnNetChannelChanged(clientIndex);
    }

    #endregion

    #region Player Listener

    /// <summary>
    /// Connect Hook
    /// </summary>
    /// <returns>0 = Accept | 1 = reject | 2 = block</returns>
    [UnmanagedCallersOnly]
    public static int OnConnectHook(
        [DNNE.C99Type("uint64_t")] ulong steamId,
        [DNNE.C99Type("const char*")] IntPtr pINetAdr,
        [DNNE.C99Type("const char*")] IntPtr pName,
        [DNNE.C99Type("const char*")] IntPtr pPassword,
        [DNNE.C99Type("char*")] IntPtr pRejectMessage,
        [DNNE.C99Type("int32_t")] int rejectMessageLength)
    {
        var name = Marshal.PtrToStringAnsi(pName);
        var iNet = Marshal.PtrToStringAnsi(pINetAdr);
        var pswd = Marshal.PtrToStringAnsi(pPassword);
        if (name is null || iNet is null || pswd is null)
        {
            return 2;
        }

        if (!IPEndPoint.TryParse(iNet, out var ipEp))
        {
            return 2;
        }

        var rejectMessage = _playerListener.OnConnectHook(steamId, ipEp, name, pswd);

        if (rejectMessage is null)
        {
            // accept
            return 0;
        }

        var bytes = Encoding.Default.GetBytes(rejectMessage);
        if (bytes.Length > rejectMessageLength)
        {
            return 2;
        }

        Marshal.Copy(bytes, 0, pRejectMessage, rejectMessageLength);
        return 1;
    }

    #endregion
}