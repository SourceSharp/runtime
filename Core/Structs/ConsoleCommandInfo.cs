using SourceSharp.Sdk.Enums;

namespace SourceSharp.Core.Structs;

public struct ConsoleCommandInfo
{
    public string Command { get; }
    public string Description { get; }
    public ConVarFlags Flags { get; }
    public AdminFlags AccessFlags { get; }

    public ConsoleCommandInfo(string command, string description, ConVarFlags flags, AdminFlags accessFlags)
    {
        Command = command;
        Description = description;
        Flags = flags;
        AccessFlags = accessFlags;
    }
}
