using SourceSharp.Core.Models;

namespace SourceSharp.Core.Interfaces;

internal interface IPlayerListener : IListenerBase
{
    /// <summary>
    /// ConnectHook - ProcessConnectionlessPacket
    /// </summary>
    /// <param name="steamId">SteamId 64</param>
    /// <param name="ip">Remote IP</param>
    /// <param name="port">Remote Port</param>
    /// <param name="name">Steam Name</param>
    /// <param name="password">Connection Password</param>
    /// <returns>返回飞空值将会阻止链接</returns>
    string? OnConnectHook(ulong steamId, string ip, ushort port, string name, string password);

    void OnConnected(CGamePlayer player);
    void OnAuthorized(CGamePlayer player);
    void OnPutInServer(CGamePlayer player);
    void OnPostAdminCheck(CGamePlayer player);
    void OnDisconnecting(CGamePlayer player);
    void OnDisconnected(CGamePlayer player);
}
