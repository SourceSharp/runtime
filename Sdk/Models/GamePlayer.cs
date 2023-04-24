using SourceSharp.Sdk.Enums;
using System;
using System.Net;
using System.Text.Json;

namespace SourceSharp.Sdk.Models;

// TODO IPlayerInfo

public abstract class GamePlayer
{
    /// <summary>
    /// 玩家名称
    /// </summary>
    public string Name { get; protected set; } = null!;

    /// <summary>
    /// IP Address
    /// </summary>
    public IPEndPoint RemoteAddress { get; }

    /// <summary>
    /// SteamId - 64
    /// </summary>
    public ulong SteamId { get; }

    /// <summary>
    /// 管理员权限
    /// </summary>
    public AdminFlags AdminFlags { get; protected set; }

    /// <summary>
    /// 是否是管理员
    /// </summary>
    public bool IsAdmin => AdminFlags != AdminFlags.None;

    /// <summary>
    /// 获取有效的SteamId
    /// </summary>
    public ulong ValidateSteamId
    {
        get
        {
            if (!IsAuthorized)
            {
                throw new InvalidOperationException("GamePlayer is unauthorized!");
            }
            return SteamId;
        }
    }

    /// <summary>
    /// UserId of engine
    /// </summary>
    public int UserId { get; }

    /// <summary>
    /// Serial number
    /// </summary>
    public uint Serial { get; }

    /// <summary>
    /// Entity index
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// 玩家是否在游戏中
    /// </summary>
    public bool IsInGame { get; protected set; }

    /// <summary>
    /// 是否是FakeClient
    /// </summary>
    public bool IsFakeClient { get; protected set; }

    /// <summary>
    /// 是否是SourceTV/GOTV
    /// </summary>
    public bool IsSourceTv { get; protected set; }

    /// <summary>
    /// 是否是Replay
    /// </summary>
    public bool IsReplay { get; protected set; }

    /// <summary>
    /// 是否已完成Steam Validation
    /// </summary>
    public bool IsAuthorized { get; protected set; }

    /// <summary>
    /// 正在退出游戏!
    /// </summary>
    public bool IsDisconnecting { get; protected set; }

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


    /*
     *  Override
     */
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

    protected GamePlayer(ulong steamId, IPEndPoint address, int userId, uint serial, int index)
    {
        SteamId = steamId;
        RemoteAddress = address;
        UserId = userId;
        Serial = serial;
        Index = index;
    }
}
