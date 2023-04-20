using System;
using System.Runtime.InteropServices;

namespace SourceSharp.Core;

internal static class Invoker
{
    [UnmanagedCallersOnly]
    public static void ConsoleCommand([DNNE.C99Type("const char*")] IntPtr commandPtr)
    {
        var command = Marshal.PtrToStringAnsi(commandPtr);
    }
}
