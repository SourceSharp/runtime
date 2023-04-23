using SourceSharp.Core.Interfaces;
using System;
using System.Runtime.InteropServices;

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
    public static void ConsoleCommand([DNNE.C99Type("const char*")] IntPtr commandPtr)
    {
        var command = Marshal.PtrToStringAnsi(commandPtr);
    }
}
