using SourceSharp.Sdk.Models;

namespace SourceSharp.Core.Interfaces;

internal interface ICommandListener : IListenerBase
{
    /// <summary>
    /// ServerConsoleCommand
    /// </summary>
    /// <param name="command">命令</param>
    /// <returns>True = 已经被Hook且已调用</returns>
    bool OnServerConsoleCommand(ConsoleCommand command);

    /// <summary>
    /// ClientConsoleCommand
    /// </summary>
    /// <param name="command">命令</param>
    /// <param name="player">调用命令的玩家</param>
    /// <returns>True = 已经被Hook且已调用</returns>
    bool OnClientConsoleCommand(ConsoleCommand command, GamePlayer? player);
}
