using SourceSharp.Sdk.Enums;
using SourceSharp.Sdk.Models;
using System.Net;

namespace SourceSharp.Core.Models;

internal sealed class CGamePlayer : GamePlayer
{
    private static uint SerialNumber;

    // construct only
    private readonly ulong _steamId;
    private readonly uint _serial;
    private readonly int _userId;
    private readonly int _index;

    // state
    private bool _disconnected;
    private bool _disconnecting;
    private bool _isInGame;
    private bool _isAuthorized;
    private bool _isFakeClient;
    private bool _isSourceTv;
    private bool _isReplay;

    // variables
    private string _name;
    private IPEndPoint _ipEndpoint;
    private AdminFlags _adminFlags;

    // internal
    private bool _inKickQueue;

    // public
    public bool IsAdminChecked { get; private set; }


    public CGamePlayer(ulong steamId, int userId, int index, string name, IPEndPoint address)
    {
        _steamId = steamId;
        _serial = ++SerialNumber; // self increase
        _userId = userId;
        _index = index;

        _name = name;
        _ipEndpoint = address;
        _adminFlags = AdminFlags.None;

        _disconnected = false;
        _disconnecting = false;
    }

    public override void Kick(string message)
    {
        if (_inKickQueue)
        {
            return;
        }

        _inKickQueue = true;

        // "kickid %d %s\n"
    }

    public override void Print(string message)
    {
        // engine->ClientPrintf(m_pEdict, pMsg);
    }

    public override void TextMsg(string message)
    {
        /*
            #define TEXTMSG_DEST_NOTIFY  1
            #define TEXTMSG_DEST_CONSOLE 2
            #define TEXTMSG_DEST_CHAT    3
            #define TEXTMSG_DEST_CENTER  4
        */
    }

    public bool Authorize(ulong steamId)
    {
        if (steamId != _steamId)
        {
            return false;
        }

        _isAuthorized = true;
        return true;
    }

    public void PutInGame(bool fakeClient, bool sourceTv, bool replay)
    {
        _isFakeClient = fakeClient;
        _isSourceTv = sourceTv;
        _isReplay = replay;

        _isInGame = true;
    }

    public void Disconnecting() => _disconnecting = true;

    public void Disconnect()
    {
        _isInGame = false;
        _disconnecting = false;
        _disconnected = true;
    }

    public void InvalidateAdmin() => _adminFlags = AdminFlags.None;

    public void AdminCheck(AdminFlags flags)
    {
        IsAdminChecked = true;
        _adminFlags = flags;
    }

    public void ChangeName(string name) => _name = name;

    /*
     * Impl
     */
    protected override ulong GetSteamId() => _steamId;
    protected override uint GetSerial() => _serial;
    protected override int GetUserId() => _userId;
    protected override int GetIndex() => _index;
    protected override string GetName() => _name;
    protected override IPEndPoint GetRemoteEndPoint() => _ipEndpoint;
    protected override AdminFlags GetAdminFlags() => _adminFlags;
    protected override bool GetIsDisconnecting() => _disconnecting;
    protected override bool GetIsDisconnected() => _disconnected;
    protected override bool GetIsInGame() => _isInGame;
    protected override bool GetIsAuthorized() => _isAuthorized;
    protected override bool GetIsFakeClient() => _isFakeClient;
    protected override bool GetIsSourceTv() => _isSourceTv;
    protected override bool GetIsReplay() => _isReplay;
}
