using SourceSharp.Sdk.Enums;
using System;
using System.Net;
using System.Text.Json;

namespace SourceSharp.Sdk.Models;

// TODO IPlayerInfo, INetChannel
// TODO CanTarget, IsAlive, IsObserver, IsTimingOut, GetInfo, GetTeam, ChangeTeam
// TODO AbsOrigin, AbsAngles ...
// TODO ExecuteCommand, ExecuteFakeCommand, ExecuteFakeCommandKeyValues

public abstract class GamePlayer
{
    /// <summary>
    /// 玩家名称
    /// </summary>
    public string Name => GetName();

    /// <summary>
    /// IP Address
    /// </summary>
    public IPEndPoint RemoteEndPoint => GetRemoteEndPoint();

    /// <summary>
    /// SteamId - 64
    /// </summary>
    public ulong SteamId => GetSteamId();

    /// <summary>
    /// 管理员权限
    /// </summary>
    public AdminFlags AdminFlags => GetAdminFlags();

    /// <summary>
    /// 是否是管理员
    /// </summary>
    public bool IsAdmin => GetAdminFlags() != AdminFlags.None;

    /// <summary>
    /// 获取有效的SteamId
    /// </summary>
    public ulong ValidateSteamId
    {
        get
        {
            if (!GetIsAuthorized())
            {
                throw new InvalidOperationException("GamePlayer is unauthorized!");
            }
            return SteamId;
        }
    }

    /// <summary>
    /// 判断该玩家是否已经离开游戏
    /// 该功能常用于异步读取数据时的判断
    /// </summary>
    public bool IsValid => !GetIsDisconnected();

    /// <summary>
    /// UserId of engine
    /// </summary>
    public int UserId => GetUserId();

    /// <summary>
    /// Serial number
    /// </summary>
    public uint Serial => GetSerial();

    /// <summary>
    /// Entity index
    /// </summary>
    public int Index => GetIndex();

    /// <summary>
    /// 玩家是否在游戏中
    /// </summary>
    public bool IsInGame => GetIsInGame();

    /// <summary>
    /// 是否是FakeClient
    /// </summary>
    public bool IsFakeClient => GetIsFakeClient();

    /// <summary>
    /// 是否是SourceTV/GOTV
    /// </summary>
    public bool IsSourceTv => GetIsSourceTv();

    /// <summary>
    /// 是否是Replay
    /// </summary>
    public bool IsReplay => GetIsReplay();

    /// <summary>
    /// 是否已完成Steam Validation
    /// </summary>
    public bool IsAuthorized => GetIsAuthorized();

    /// <summary>
    /// 正在退出游戏!
    /// </summary>
    public bool IsDisconnecting => GetIsDisconnecting();

    /// <summary>
    /// 打印到控制台
    /// </summary>
    /// <param name="message">内容</param>
    public abstract void Print(string message);

    /// <summary>
    /// 发送TextMsg
    /// </summary>
    /// <param name="message">内容</param>
    public abstract void TextMsg(string message);

    /// <summary>
    /// 踢出游戏
    /// </summary>
    public abstract void Kick(string message);

    #region override

    private readonly Guid _uniqueId = Guid.NewGuid();

    public override int GetHashCode()
        => _uniqueId.GetHashCode();

    public override bool Equals(object? obj)
    {
        if (obj is GamePlayer p2)
        {
            return _uniqueId == p2._uniqueId;
        }

        return false;
    }

    public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions
    {
        WriteIndented = true,
        IncludeFields = false,
    });

    #endregion

    #region internal implements

    protected abstract ulong GetSteamId();
    protected abstract uint GetSerial();
    protected abstract int GetUserId();
    protected abstract int GetIndex();
    protected abstract string GetName();
    protected abstract IPEndPoint GetRemoteEndPoint();
    protected abstract AdminFlags GetAdminFlags();

    protected abstract bool GetIsDisconnecting();
    protected abstract bool GetIsDisconnected();
    protected abstract bool GetIsInGame();
    protected abstract bool GetIsAuthorized();
    protected abstract bool GetIsFakeClient();
    protected abstract bool GetIsSourceTv();
    protected abstract bool GetIsReplay();
    #endregion
}
