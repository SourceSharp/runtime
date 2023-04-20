using System;

namespace SourceSharp.Sdk.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class PluginAttribute : Attribute
{
    public required string Name { get; set; }
    public required string Author { get; set; }
    public required string Version { get; set; }
    public string Url { get; set; } = "https://github.com/SourceSharp";
    public string Description { get; set; } = string.Empty;
}