using System;
using System.Collections.Generic;

namespace SourceSharp.Core.Models;

public interface IGameEvent
{
    T Get<T>(string key);
}

internal class GameEvent
{
    private readonly Dictionary<string, object> _dictionary = new();

    public object this[string index]
    {
        get => _dictionary[index];
        set
        {
            _dictionary[index] = value;
            throw new NotImplementedException();
        }
    }

    public T Get<T>(string key)
    {
        if (!_dictionary.TryGetValue(key, out var value))
        {
            throw new InvalidOperationException($"Key '{key}' does not exists.");
        }

        if (value is not T v)
        {
            throw new InvalidOperationException($"Key '{key}' is not type {typeof(T).Name}");
        }

        return v;
    }
}
