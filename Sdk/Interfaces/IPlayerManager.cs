using SourceSharp.Sdk.Models;
using System.Collections.Generic;

namespace SourceSharp.Sdk.Interfaces;

public interface IPlayerManager : IRuntime
{
    /// <summary>
    /// 获取GamePlayer实例
    /// </summary>
    /// <param name="index">Client Index</param>
    /// <returns>GamePlayer实例</returns>
    GamePlayer? GetGamePlayer(int index);

    /// <summary>
    /// 获取GamePlayer实例
    /// </summary>
    /// <param name="serial">Client Serial</param>
    /// <returns>GamePlayer实例</returns>
    GamePlayer? GetGamePlayer(uint serial);

    /// <summary>
    /// 获取GamePlayer实例
    /// </summary>
    /// <param name="userId">userId</param>
    /// <returns>Client Index</returns>
    GamePlayer? GetGamePlayerByUserId(int userId);

    /// <summary>
    /// 获取玩家列表
    /// (不包含SourceTV, Replay)
    /// </summary>
    /// <returns>GamePlayer实例列表</returns>
    IReadOnlyList<GamePlayer> GetPlayers();

    /// <summary>
    /// 获取玩家列表
    /// (Player, FakeClient, Replay, SourceTV)
    /// </summary>
    /// <returns>GamePlayer实例列表</returns>
    IReadOnlyList<GamePlayer> GetClients();

    /// <summary>
    /// 获取游戏内允许的最大人数
    /// </summary>
    /// <returns>人数</returns>
    uint GetMaxClients();

    /// <summary>
    /// 获取当前已建立连接的人数
    /// </summary>
    /// <returns>人数</returns>
    uint GetNumPlayers();
}
