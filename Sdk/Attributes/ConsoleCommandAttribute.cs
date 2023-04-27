using SourceSharp.Sdk.Enums;
using System;

namespace SourceSharp.Sdk.Attributes;

public abstract class ConsoleCommandBaseAttribute : HookBaseAttribute<string>
{
    public string Description { get; }
    public ConVarFlags Flags { get; }
    public string Command => Key;

    protected ConsoleCommandBaseAttribute(string command) : base(command.ToLower())
    {
        Description = string.Empty;
        Flags = ConVarFlags.None;
    }

    protected ConsoleCommandBaseAttribute(string command, string description) : base(command.ToLower())
    {
        Description = description;
        Flags = ConVarFlags.None;
    }

    protected ConsoleCommandBaseAttribute(string command, string description, ConVarFlags flags) : base(command.ToLower())
    {
        Description = description;
        Flags = flags;
    }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class ServerConsoleCommandAttribute : ConsoleCommandBaseAttribute
{
    public ServerConsoleCommandAttribute(string command) : base(command) { }

    public ServerConsoleCommandAttribute(string command, string description) : base(command, description) { }

    public ServerConsoleCommandAttribute(string command, string description, ConVarFlags flags) : base(command, description, flags) { }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class ClientConsoleCommandAttribute : ConsoleCommandBaseAttribute
{
    public AdminFlags AccessFlags { get; }

    public ClientConsoleCommandAttribute(string command) : base(command)
    {
        AccessFlags = AdminFlags.None;
    }

    public ClientConsoleCommandAttribute(string command, string description) : base(command, description)
    {
        AccessFlags = AdminFlags.None;
    }

    public ClientConsoleCommandAttribute(string command, string description, ConVarFlags flags) : base(command, description, flags)
    {
        AccessFlags = AdminFlags.None;
    }

    public ClientConsoleCommandAttribute(string command, string description, ConVarFlags flags, AdminFlags admin) : base(command, description, flags)
    {
        AccessFlags = admin;
    }
}
