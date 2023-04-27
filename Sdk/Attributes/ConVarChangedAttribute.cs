using System;

namespace SourceSharp.Sdk.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class ConVarChangedAttribute : Attribute
{
    public string Name { get; }

    public ConVarChangedAttribute(string name)
    {
        Name = name.ToLower();
    }
}
