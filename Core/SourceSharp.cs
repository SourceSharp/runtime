using SourceSharp.Core.Interfaces;
using System;
using System.Collections.Concurrent;

namespace SourceSharp.Core;

internal sealed class SourceSharp : SourceSharpBase
{
    private readonly int _masterThreadId;
    private readonly ConcurrentQueue<Action> _invokes;

    public SourceSharp()
    {
        _masterThreadId = Environment.CurrentManagedThreadId;
        _invokes = new();
    }

    public override void RunFrame()
    {
        while (_invokes.TryDequeue(out var action))
        {
            action.Invoke();
        }
    }

    public override void Invoke(Action action)
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

    public override void InvokeNextFrame(Action action)
        => _invokes.Enqueue(action);
}
