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


}
