﻿using System;
using System.Collections.Generic;

namespace SourceSharp.Sdk.Models;

public class GameEvent
{
    private readonly Dictionary<string, object> _dictionary = new();

    public string Name { get; } = null!;

    public object this[string index]
    {
        get => _dictionary[index];
        set => throw new NotImplementedException();
    }

    // bool,  int, float, string
    // sbyte, int, float, string

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
