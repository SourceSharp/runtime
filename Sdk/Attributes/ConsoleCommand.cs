﻿using SourceSharp.Sdk.Enums;
using System;

namespace SourceSharp.Sdk.Attributes;

public abstract class ConsoleCommandBase : Attribute
{
    public string Command { get; }
    public string Description { get; }
    public ConVarFlags Flags { get; }

    protected ConsoleCommandBase(string command)
    {
        Command = command;
        Description = string.Empty;
        Flags = ConVarFlags.None;
    }

    protected ConsoleCommandBase(string command, string description)
    {
        Command = command;
        Description = description;
        Flags = ConVarFlags.None;
    }

    protected ConsoleCommandBase(string command, string description, ConVarFlags flags)
    {
        Command = command;
        Description = description;
        Flags = flags;
    }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class ServerConsoleCommand : ConsoleCommandBase
{
    public ServerConsoleCommand(string command) : base(command) { }

    public ServerConsoleCommand(string command, string description) : base(command, description) { }

    public ServerConsoleCommand(string command, string description, ConVarFlags flags) : base(command, description, flags) { }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class ClientConsoleCommand : ConsoleCommandBase
{
    public AdminFlags AccessFlags { get; }

    public ClientConsoleCommand(string command) : base(command)
    {
        AccessFlags = AdminFlags.None;
    }

    public ClientConsoleCommand(string command, string description) : base(command, description)
    {
        AccessFlags = AdminFlags.None;
    }

    public ClientConsoleCommand(string command, string description, ConVarFlags flags) : base(command, description, flags)
    {
        AccessFlags = AdminFlags.None;
    }

    public ClientConsoleCommand(string command, string description, ConVarFlags flags, AdminFlags admin) : base(command, description, flags)
    {
        AccessFlags = admin;
    }
}
