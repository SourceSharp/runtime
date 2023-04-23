namespace SourceSharp.Sdk.Models;

public sealed class ConsoleCommand
{
    public string[] Args { get; }
    public string ArgString { get; }
    public int ArgC { get; }

    public ConsoleCommand(string argString, string[] args, int argC)
    {
        ArgString = argString;
        Args = args;
        ArgC = argC;
    }
}
