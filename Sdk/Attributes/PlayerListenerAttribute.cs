using SourceSharp.Sdk.Enums;
using System;

namespace SourceSharp.Sdk.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class PlayerListenerAttribute : Attribute
{
    public PlayerListenerType Type { get; }

    public PlayerListenerAttribute(PlayerListenerType type)
        => Type = type;
}
