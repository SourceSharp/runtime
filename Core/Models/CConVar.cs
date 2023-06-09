using SourceSharp.Core.Bridges;
using SourceSharp.Sdk.Enums;
using SourceSharp.Sdk.Interfaces;
using SourceSharp.Sdk.Models;
using SourceSharp.Sdk.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using ConVar = SourceSharp.Sdk.Models.ConVar;

namespace SourceSharp.Core.Models;

internal record ConVarHook(IPlugin Plugin, Action<ConVar, string, string> Callback);

internal sealed class CConVar : ConVar
{

    private readonly SSConVar _conVar;
    private readonly string _name;
    private readonly string _description;
    private readonly List<ConVarHook> _callbacks;

    public CConVar(SSConVar conVar, string name, string description)
    {
        _conVar = conVar;
        _name = name;
        _description = description;

        _callbacks = new();
    }

    public override T Get<T>()
    {
        if (typeof(T) == typeof(int))
        {
            return (T)Convert.ChangeType(_conVar.Int, typeof(T));
        }
        if (typeof(T) == typeof(bool))
        {
            return (T)Convert.ChangeType(_conVar.Bool, typeof(T));
        }
        if (typeof(T) == typeof(float))
        {
            return (T)Convert.ChangeType(_conVar.Float, typeof(T));
        }
        if (typeof(T) == typeof(string))
        {
            return (T)Convert.ChangeType(_conVar.String, typeof(T));
        }

        throw new InvalidCastException($"Bad ValueType<{typeof(T).FullName}> for event.");
    }

    public override void Set<T>(T value)
    {
        switch (value)
        {
            case int iV:
                _conVar.Int = iV;
                break;
            case bool bV:
                _conVar.Bool = bV;
                break;
            case float flV:
                _conVar.Float = flV;
                break;
            case string sV:
                _conVar.String = sV;
                break;
            default:
                throw new InvalidCastException($"Bad ValueType<{typeof(T).FullName}> for event.");
        }
    }

    public override bool ReplicateToPlayers(GamePlayer[] players)
    {
        var playerIndex = players
            .Where(x => x is { IsValid: true, IsFakeClient: false, IsSourceTv: false, IsReplay: false })
            .Select(x => x.Index)
            .ToArray();

        return _conVar.ReplicateToPlayers(playerIndex, playerIndex.Length);
    }

    public override void AddFlags(ConVarFlags flags) => _conVar.AddFlags((int)flags);

    public override void RegisterChangeHook(IPlugin plugin, Action<ConVar, string, string> callback)
    {
        Bridges.ConVar.RegisterConVarHook(_name);
        _callbacks.Add(new ConVarHook(plugin, callback));
    }

    internal void Invoke(string oldValue, string newValue) =>
        _callbacks.ForEach(hook =>
        {
            // TODO plugin status check

            try
            {
                hook.Callback.Invoke(this, oldValue, newValue);
            }
            catch { }
        });

    protected override string GetName() => _name;

    protected override string GetDescription() => _description;

    protected override string GetDefaultValue() => _conVar.Default;

    protected override ConVarFlags GetFlags() => (ConVarFlags)_conVar.Flags;

    protected override ConVarBounds GetBounds() => new(_conVar.MinValue, _conVar.MaxValue, _conVar.HasMin, _conVar.HasMax);

    protected override void SetBounds(ConVarBounds bounds)
    {
#if false
        if (bounds.HasMin)
        {
            _conVar.HasMin = true;
            _conVar.Min = bounds.Min;
        }
        else
        {
            _conVar.HasMin = false;
        }

        if (bounds.HasMax)
        {
            _conVar.HasMax = true;
            _conVar.Max = bounds.Max;
        }
        else
        {
            _conVar.HasMax = false;
        }
#endif

        // TODO
        throw new NotImplementedException();
    }

    // Internal Api
    internal void RemoveHook(IPlugin iPlugin)
    {
        var hooks = _callbacks.Where(x => x.Plugin == iPlugin);
        foreach (var hook in hooks)
        {
            _callbacks.Remove(hook);
        }
    }
}
