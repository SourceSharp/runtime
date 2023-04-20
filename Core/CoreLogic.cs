using SourceSharp.Sdk.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace SourceSharp.Core;

internal class CoreLogic : ICore
{
    public readonly ConcurrentQueue<Action> _invokeActions = new();

    private static class Natives
    {
        [DllImport("Extension.dll", CharSet = CharSet.Unicode)]
        public static extern void PrintLine(string msg);

        [DllImport("Extension.dll", CharSet = CharSet.Unicode)]
        public static extern void LogMessage(string msg);

        [DllImport("Extension.dll", CharSet = CharSet.Unicode)]
        public static extern void LogError(string msg);

        [DllImport("Extension.dll", SetLastError = true)]
        public static extern void RegServerCommand(string command);
    }

    public void PrintLine(string message) => Natives.PrintLine(message);
    public void LogMessage(string message) => Natives.LogMessage(message);
    public void LogError(string message) => Natives.LogError(message);
    public void Invoke(Action action) => _invokeActions.Enqueue(action);


}
