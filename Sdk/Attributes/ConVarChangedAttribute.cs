using System;

namespace SourceSharp.Sdk.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class ConVarChangedAttribute : HookBaseAttribute<string>
{
    public string Name => Key;

    public ConVarChangedAttribute(string name) : base(name.ToLower()) { }
}
