
using SourceSharp.Core.Interfaces;
using System;
using System.Runtime.InteropServices;
using SourceSharp.Sdk.Models;

namespace SourceSharp.Core;

internal static class Invoker
{
#nullable disable
    private static IPluginManager _pluginManager;
    private static ISourceSharpBase _sourceSharp;
#nullable restore

    internal static void Initialize(IPluginManager pluginManager, ISourceSharpBase sourceSharp)
    {
        _pluginManager = pluginManager;
        _sourceSharp = sourceSharp;
    }

    [UnmanagedCallersOnly]
    public static void OnGameFrame(bool simulating, int tickCount, float gameTime)
    {
        _sourceSharp.RunFrame(tickCount, gameTime);
        _pluginManager.OnGameFrame(simulating);
    }

    [UnmanagedCallersOnly]
    public static void ConsoleCommand([DNNE.C99Type("const char*")] IntPtr argStringPtr,
        [DNNE.C99Type("const char**")] IntPtr argsPtr,
        int argc)
    {
        var args = Enumerable.Range(0, argc - 1).Select(index =>
        {
            if (Marshal.PtrToStructure(IntPtr.Add(argsPtr, index), typeof(IntPtr)) is IntPtr strPtr)
            {
                var result = Marshal.PtrToStringAnsi(strPtr);

                if (result is not null)
                {
                    return result;
                }
            }

            return string.Empty;
        }).ToArray();

        var command = new ConsoleCommand
        {
            Args = args,
            ArgString = Marshal.PtrToStringAnsi(argStringPtr),
            ArgC = argc
        };

        Console.WriteLine(command.ArgC);
        Console.WriteLine(command.Args[0]);
        Console.WriteLine(command.ArgString);
    }
}