using SourceSharp.Sdk.Enums;
using System;

namespace SourceSharp.Sdk.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class GameEventAttribute : Attribute
{
    public string Name { get; }
    public GameEventHookType Type { get; }

    public GameEventAttribute(string name)
    {
        Name = name.ToLower();
        Type = GameEventHookType.Post;
    }

    public GameEventAttribute(string name, GameEventHookType type)
    {
        Name = name.ToLower();
        Type = type;
    }
}
