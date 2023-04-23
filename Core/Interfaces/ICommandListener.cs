using SourceSharp.Sdk.Models;

namespace SourceSharp.Core.Interfaces;

internal interface ICommandListener : IModuleBase
{
    void OnServerConsoleCommand(ConsoleCommand command);

    void OnClientConsoleCommand(ConsoleCommand command, GamePlayer? player);
}
