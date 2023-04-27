using SourceSharp.Core.Bridges;
using SourceSharp.Sdk.Enums;
using SourceSharp.Sdk.Models;
using SourceSharp.Sdk.Structs;
using System;
using System.Linq;
using ConVar = SourceSharp.Sdk.Models.ConVar;

namespace SourceSharp.Core.Models;

internal class CConVar : ConVar
{
    private readonly IConVar _conVar;
    private readonly string _name;
    private readonly string _description;

    public CConVar(IConVar conVar, string name, string description)
    {
        _conVar = conVar;
        _name = name;
        _description = description;
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

    protected override string GetName() => _name;

    protected override string GetDescription() => _description;

    protected override string GetDefaultValue() => _conVar.Default;

    protected override ConVarFlags GetFlags() => (ConVarFlags)_conVar.Flags;

    protected override void SetFlags(ConVarFlags flags) => _conVar.Flags = (int)flags;

    protected override ConVarBounds GetBounds() => new(_conVar.Min, _conVar.Max, _conVar.HasMin, _conVar.HasMax);

    protected override void SetBounds(ConVarBounds bounds)
    {
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
    }
}
