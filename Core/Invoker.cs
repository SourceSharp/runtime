using Microsoft.Extensions.DependencyInjection;
using SourceSharp.Core.Interfaces;
using SourceSharp.Sdk.Models;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace SourceSharp.Core;

internal static class Invoker
{
#nullable disable
    private static IPluginManager _pluginManager;
    private static ISourceSharpBase _sourceSharp;
    private static ICommandListener _commandListener;
    private static IPlayerManagerBase _playerManager;
#nullable restore

    internal static void Initialize(IServiceProvider services)
    {
        _pluginManager = services.GetRequiredService<IPluginManager>();
        _sourceSharp = services.GetRequiredService<ISourceSharpBase>();
        _commandListener = services.GetRequiredService<ICommandListener>();
        _playerManager = services.GetRequiredService<IPlayerManagerBase>();
    }

    /*
     * Bool 不是可导出/导入的类型!
     */

    [UnmanagedCallersOnly]
    public static void OnGameFrame(sbyte simulating, int tickCount, float gameTime)
    {
        _sourceSharp.RunFrame(tickCount, gameTime);
        _pluginManager.OnGameFrame(simulating > 0);
    }

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
            if (Marshal.PtrToStructure(IntPtr.Add(pArgs, index), typeof(IntPtr)) is IntPtr strPtr)
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
}