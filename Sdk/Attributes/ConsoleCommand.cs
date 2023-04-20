using System;

namespace SourceSharp.Sdk.Attributes;


[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class ServerConsoleCommand : Attribute
{
    public required string Command { get; set; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class ClientConsoleCommand : Attribute
{
    public required string Command { get; set; }
}
