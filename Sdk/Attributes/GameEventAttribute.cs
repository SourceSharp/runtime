using System;

namespace SourceSharp.Sdk.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class GameEventAttribute : Attribute
{
    public required string Name { get; set; }

    public GameEventAttribute(string name)
        => Name = name.ToLower();
}
