using SourceSharp.Sdk.Enums;
using System;

namespace SourceSharp.Sdk.Models;

public abstract class AdminUser
{
    public uint Id { get; protected set; }
    public string Name { get; protected set; } = "TempAdmin::" + Guid.NewGuid();
    public ulong SteamId { get; protected set; }
    public AdminFlags Flags => _accessFlags;

    private AdminFlags _accessFlags = AdminFlags.None;

    /// <summary>
    /// 覆盖Flags
    /// </summary>
    /// <param name="flags">新的Flags</param>
    public void SetAdminFlags(AdminFlags flags) => _accessFlags = flags;
     
    /// <summary>
    /// 新增Access Flags
    /// </summary>
    /// <param name="flags">要增加的Flags</param>
    public void AddAdminFlags(AdminFlags flags) => _accessFlags |= flags;

    /// <summary>
    /// 清除Access Flags
    /// </summary>
    /// <param name="flags">要删除的Flags</param>
    public void RemoveAdminFlags(AdminFlags flags) => _accessFlags &= ~flags;

    /// <summary>
    /// 检查Access Flags
    /// </summary>
    /// <param name="flags">要检查的Flags</param>
    /// <returns>拥有指定的Flags</returns>
    public bool HasFlags(AdminFlags flags) => _accessFlags.HasFlag(flags);
}