using System;
using System.Linq;
using System.Runtime.InteropServices;
using SourceSharp.Sdk.Models;

namespace SourceSharp.Core;

internal static class Invoker
{
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