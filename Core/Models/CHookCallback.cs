using System;
using System.Reflection;

namespace SourceSharp.Core.Models;

internal abstract class CHookCallback<T> where T : Delegate
{
    public CPlugin Plugin { get; }
    public T Callback { get; }

    protected CHookCallback(CPlugin plugin, MethodInfo method)
    {
        Plugin = plugin;
        Callback = method.CreateDelegate<T>();
    }
}