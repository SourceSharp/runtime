using SourceSharp.Sdk.Models;
using System.Collections.Generic;

namespace SourceSharp.Sdk.Interfaces;

// TODO OnRebuildAdminCache  OnAdminChanged

public interface IAdminManager : IRuntime
{
    /// <summary>
    /// 创建管理员
    /// </summary>
    /// <param name="steamId">Steam 64位 ID</param>
    /// <param name="name"></param>
    /// <returns>AdminUser实例</returns>
    AdminUser CreateAdmin(ulong steamId, string name);

    /// <summary>
    /// 注销管理员
    /// </summary>
    /// <param name="admin">AdminUser实例</param>
    void RemoveAdmin(AdminUser admin);

    /// <summary>
    /// 获取所有管理员
    /// </summary>
    /// <returns>IReadOnlyList</returns>
    IReadOnlyList<AdminUser> GetAdmins();

    /// <summary>
    /// 通过SteamId搜索AdminUser实例
    /// </summary>
    /// <param name="steamId">Steam 64位 ID</param>
    /// <returns>AdminUser实例</returns>
    AdminUser? FindAdminByIdentity(ulong steamId);
}
