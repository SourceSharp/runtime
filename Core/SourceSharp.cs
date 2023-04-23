using SourceSharp.Core.Interfaces;
using System;
using System.Collections.Concurrent;

namespace SourceSharp.Core;

internal sealed class SourceSharp : SourceSharpBase
{
    private readonly int _masterThreadId;
    private readonly ConcurrentQueue<Action> _invokes;

    private int _tickCount;
    private float _gameTime;

    public SourceSharp()
    {
        _masterThreadId = Environment.CurrentManagedThreadId;
        _invokes = new();
    }

    public override void RunFrame(int tickCount, float gameTime)
    {
        _tickCount = tickCount;
        _gameTime = gameTime;

        while (_invokes.TryDequeue(out var action))
        {
            action.Invoke();
        }
    }
}
