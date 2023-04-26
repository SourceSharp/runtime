using SourceSharp.Core.Bridges;
using SourceSharp.Sdk.Models;
using System;
using System.Collections.Generic;

namespace SourceSharp.Core.Models;

internal class CGameEvent : GameEvent
{
    private readonly Dictionary<string, object> _values;
    private readonly IGameEvent _event;
    private readonly string _eventName;
    private readonly bool _noEdit;
    private bool _broadcast;

    public CGameEvent(IGameEvent @event, bool post)
    {
        _event = @event;
        _eventName = @event.Name;
        _broadcast = @event.Broadcast;

        _values = new();
        _noEdit = post;
    }

    public override T Get<T>(string key)
    {
        if (_values.TryGetValue(key, out var value))
        {
            if (value is not T v)
            {
                throw new InvalidOperationException($"Key '{key}' is not type {typeof(T).Name}");
            }

            return v;
        }

        T newValue;

        // Call from native
        if (typeof(T) == typeof(int))
        {
            newValue = (T)Convert.ChangeType(_event.GetInt(key), typeof(T));
        }
        else if (typeof(T) == typeof(bool))
        {
            newValue = (T)Convert.ChangeType(_event.GetBool(key), typeof(T));
        }
        else if (typeof(T) == typeof(float))
        {
            newValue = (T)Convert.ChangeType(_event.GetFloat(key), typeof(T));
        }
        else if (typeof(T) == typeof(string))
        {
            newValue = (T)Convert.ChangeType(_event.GetString(key), typeof(T));
        }
        else
        {
            throw new InvalidCastException($"Bad ValueType<{typeof(T).FullName}> for event.");
        }

        _values[key] = newValue;

        return newValue;
    }

    public override bool Set<T>(string key, T value)
    {
        CheckEditable();

        var retV = value switch
        {
            int v => _event.SetInt(key, v),
            bool v => _event.SetBool(key, v),
            float v => _event.SetFloat(key, v),
            string v => _event.SetString(key, v),
            _ => throw new InvalidCastException($"Bad ValueType<{typeof(T).FullName}> for event.")
        };

        if (retV)
        {
            _values[key] = value;
        }

        return retV;
    }

    protected override string GetName() => _eventName;
    protected override bool GetBroadcast() => _broadcast;
    protected override void SetBroadcast(bool broadcast)
    {
        CheckEditable();

        _broadcast = broadcast;
        _event.Broadcast = broadcast;
    }

    private void CheckEditable()
    {
        if (_noEdit)
        {
            throw new InvalidOperationException("Event can not change after fired.");
        }
    }

    public override int GetHashCode() => _event.__Instance.GetHashCode();
    public override bool Equals(object? obj)
    {
        if (obj is CGameEvent e)
        {
            return e.GetHashCode() == GetHashCode();
        }

        return false;
    }
}
