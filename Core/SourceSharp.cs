using SourceSharp.Sdk.Enums;
using SourceSharp.Sdk.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace SourceSharp.Core;

internal sealed class SourceSharp : ISourceSharp
{
    private readonly int _masterThreadId;
    private readonly ConcurrentQueue<Action> _invokes;

    public SourceSharp()
    {
        _masterThreadId = Environment.CurrentManagedThreadId;
        _invokes = new();
    }

    /*
     *  IRuntime
     */

    public string GetInterfaceName() => "ISOURCESHARP_CORE";
    public uint GetInterfaceVersion() => (uint)Assembly.GetExecutingAssembly()!.GetName()!.Version!.Major;


    /*
     * ISourceSharp
     */
    public void Invoke(Action action)
    {
        if (Environment.CurrentManagedThreadId == _masterThreadId)
        {
            action.Invoke();
        }
        else
        {
            _invokes.Enqueue(action);
        }
    }

    public void InvokeNextFrame(Action action) => _invokes.Enqueue(action);

    public string BuildPath(PathType type, params string[] format)
    {
        throw new NotImplementedException();
    }

    public string GetGamePath()
    {
        throw new NotImplementedException();
    }

    public string GetSourceSharpPath()
    {
        throw new NotImplementedException();
    }

    public void LogError(string message)
    {
        throw new NotImplementedException();
    }

    public void LogMessage(string message)
    {
        throw new NotImplementedException();
    }

    public void PrintLine(string message)
    {
        throw new NotImplementedException();
    }
}
