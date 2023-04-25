using SourceSharp.Sdk.Enums;

namespace SourceSharp.Sdk.Models;

public abstract class AdminUser
{
    /// <summary>
    /// 管理员Id
    /// </summary>
    public uint Id => GetId();

    /// <summary>
    /// 管理员名字
    /// </summary>
    public string Name => GetName();

    /// <summary>
    /// 管理员SteamId
    /// </summary>
    public ulong SteamId => GetSteamId();

    /// <summary>
    /// 管理员AdminFlags
    /// </summary>
    public AdminFlags Flags => GetFlags();

    /// <summary>
    /// 覆盖Flags
    /// </summary>
    /// <param name="flags">新的Flags</param>
    public abstract void SetAdminFlags(AdminFlags flags);

    /// <summary>
    /// 新增Access Flags
    /// </summary>
    /// <param name="flags">要增加的Flags</param>
    public abstract void AddAdminFlags(AdminFlags flags);

    /// <summary>
    /// 清除Access Flags
    /// </summary>
    /// <param name="flags">要删除的Flags</param>
    public abstract void RemoveAdminFlags(AdminFlags flags);

    /// <summary>
    /// 检查Access Flags
    /// </summary>
    /// <param name="flags">要检查的Flags</param>
    /// <returns>拥有指定的Flags</returns>
    public abstract bool HasFlags(AdminFlags flags);

    #region internal implements

    protected abstract uint GetId();
    protected abstract string GetName();
    protected abstract ulong GetSteamId();
    protected abstract AdminFlags GetFlags();

    #endregion
}