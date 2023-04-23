using SourceSharp.Sdk.Models;

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
    /// <returns></returns>
    GamePlayer? GetGamePlayer(uint serial);

    /// <summary>
    /// 获取Client Index
    /// </summary>
    /// <param name="userId">userId</param>
    /// <returns>Client Index</returns>
    int GetClientOfUserId(int userId);

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
