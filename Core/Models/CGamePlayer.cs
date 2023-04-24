using SourceSharp.Core.Interfaces;
using SourceSharp.Sdk.Models;
using System.Net;

namespace SourceSharp.Core.Models;

internal sealed class CGamePlayer : GamePlayer
{
    private readonly IPlayerManagerBase _playerManager;
    private readonly IAdminManagerBase _adminManager;

    private bool _inKickQueue;

    public CGamePlayer(IPlayerManagerBase playerManager, IAdminManagerBase adminManager,
        string name, IPEndPoint address, int userId, ulong steamId, uint serial, int index)
    {
        _playerManager = playerManager;
        _adminManager = adminManager;

        Name = name;
        RemoteAddress = address;
        UserId = userId;
        SteamId = steamId;
        Serial = serial;
        Index = index;
    }

    public override void Kick(Sdk.Models.GamePlayer player)
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

    public override void RunAdminCacheChecks()
        => _adminManager.CheckGameUserAdmin(this);
}
