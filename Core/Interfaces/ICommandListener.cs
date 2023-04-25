using SourceSharp.Sdk.Models;

namespace SourceSharp.Core.Interfaces;

internal interface ICommandListener : IListenerBase
{
    void OnServerConsoleCommand(ConsoleCommand command);

    void OnClientConsoleCommand(ConsoleCommand command, GamePlayer? player);
}
