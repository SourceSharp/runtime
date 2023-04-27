using SourceSharp.Sdk.Enums;
using System;

namespace SourceSharp.Sdk.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ConVarAttribute : Attribute
{
    public string Name { get; }
    public string DefaultValue { get; }
    public string Description { get; set; } = string.Empty;
    public ConVarFlags Flags { get; set; } = ConVarFlags.None;
    public bool HasMin { get; set; }
    public float Min { get; set; }
    public bool HasMax { get; set; }
    public float Max { get; set; }

    public ConVarAttribute(string name, string defaultValue)
    {
        Name = name.ToLower();
        DefaultValue = defaultValue;
    }
}
