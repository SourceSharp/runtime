using SourceSharp.Sdk.Enums;
using SourceSharp.Sdk.Models;
using System.Net;

namespace SourceSharp.Core.Models;

internal sealed class CGamePlayer : GamePlayer
{
    private bool _inKickQueue;

    public bool IsAdminChecked { get; private set; }

    public CGamePlayer(string name, IPEndPoint address, int userId, ulong steamId, uint serial, int index)
        : base(steamId, address, userId, serial, index)
    {
        Name = name;
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
        if (steamId != SteamId)
        {
            return false;
        }

        IsAuthorized = true;
        return true;
    }

    public void PutInGame(bool fakeClient, bool sourceTv, bool replay)
    {
        IsFakeClient = fakeClient;
        IsSourceTv = sourceTv;
        IsReplay = replay;

        IsInGame = true;
    }

    public void Disconnecting() => IsDisconnecting = true;

    public void Disconnect()
    {
        IsInGame = false;
    }

    public void InvalidateAdmin() => AdminFlags = AdminFlags.None;

    public void AdminCheck(AdminFlags flags)
    {
        IsAdminChecked = true;
        AdminFlags = flags;
    }

    public void ChangeName(string name) => Name = name;
}
