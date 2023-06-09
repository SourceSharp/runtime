using Microsoft.Extensions.DependencyInjection;
using SourceSharp.Core.Interfaces;
using SourceSharp.Sdk.Models;
using System;
using System.Collections.Concurrent;

namespace SourceSharp.Core;

internal sealed class SourceSharp : SourceSharpBase
{
    private readonly int _masterThreadId;
    private readonly ConcurrentQueue<Action> _invokes;
    private readonly IServiceProvider _service;

    public SourceSharp(IServiceProvider service)
    {
        _masterThreadId = Environment.CurrentManagedThreadId;
        _invokes = new();
        _service = service;
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

    public override ConVar? FindConVar(string name)
    {
        var cm = _service.GetRequiredService<IConVarManager>();
        return cm.FindConVar(name);
    }

    public override void InvokeNextFrame(Action action)
        => _invokes.Enqueue(action);
}
