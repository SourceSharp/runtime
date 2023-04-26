using SourceSharp.Sdk.Enums;
using System;

namespace SourceSharp.Sdk.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class GameEventAttribute : Attribute
{
    public string Name { get; }
    public GameEventHookType Type { get; }
    public int Priority { get; set; }

    public GameEventAttribute(string name)
    {
        Name = name.ToLower();
        Type = GameEventHookType.Post;
        Priority = 0;
    }

    public GameEventAttribute(string name, GameEventHookType type)
    {
        Name = name.ToLower();
        Type = type;
        Priority = 0;
    }

    public GameEventAttribute(string name, GameEventHookType type, int priority)
    {
        Name = name.ToLower();
        Type = type;
        Priority = int.Clamp(priority, 0, 100);
    }
}
