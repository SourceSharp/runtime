using SourceSharp.Core.Interfaces;
using SourceSharp.Core.Models;
using SourceSharp.Sdk;
using SourceSharp.Sdk.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace SourceSharp.Core.Modules;

internal sealed class PlayerManager : IPlayerManagerBase
{
    private readonly ISourceSharpBase _sourceSharp;
    private readonly IPlayerListener _playerListener;
    private readonly IAdminManagerBase _adminManager;

    // TODO GetMaxClients
    private readonly List<CGamePlayer> _players;

    private uint _serialNumber;

    public PlayerManager(ISourceSharpBase sourceSharp, IPlayerListener playerListener, IAdminManagerBase adminManager)
    {
        _sourceSharp = sourceSharp;
        _playerListener = playerListener;
        _adminManager = adminManager;

        _players = new(64);
        _serialNumber = 0;
    }

    /*
     * IPlayerManagerBase
     */
    public void OnConnected(int clientIndex, int userId, ulong steamId, string name, IPEndPoint remoteEndpoint)
    {
        var player = new CGamePlayer(name, remoteEndpoint, userId, steamId, ++_serialNumber, clientIndex);
        _players.Add(player);
        _playerListener.OnConnected(player);
    }

    public void OnAuthorized(int clientIndex, ulong steamId)
    {
        var player = _players.SingleOrDefault(x => x.Index == clientIndex);
        if (player is null)
        {
            _sourceSharp.LogError($"Authorized failed: player index {clientIndex} not found");
            return;
        }

        if (!player.Authorize(steamId))
        {
            _sourceSharp.LogError($"Authorized failed: player index {clientIndex} steamId is spoofed");
            player.Kick("SteamId Spoof!");
            return;
        }

        _playerListener.OnAuthorized(player);

        if (player is { IsInGame: true, IsAdminChecked: false })
        {
            RunAdminCheck(player, true);
        }
    }

    public void OnPutInServer(int clientIndex, bool fakeClient, bool sourceTv, bool replay)
    {
        var player = _players.SingleOrDefault(x => x.Index == clientIndex);
        if (player is null)
        {
            _sourceSharp.LogError($"PutInServer failed: player index {clientIndex} not found");
            return;
        }

        player.PutInGame(fakeClient, sourceTv, replay);

        _playerListener.OnPutInServer(player);

        if (player is { IsAuthorized: true, IsAdminChecked: false })
        {
            RunAdminCheck(player, true);
        }
    }

    public void OnDisconnecting(int clientIndex)
    {
        var player = _players.SingleOrDefault(x => x.Index == clientIndex);
        if (player is null)
        {
            _sourceSharp.LogError($"Disconnecting failed: player index {clientIndex} not found");
            return;
        }

        player.Disconnecting();

        _playerListener.OnDisconnecting(player);
    }

    public void OnDisconnected(int clientIndex)
    {
        var player = _players.SingleOrDefault(x => x.Index == clientIndex);
        if (player is null)
        {
            _sourceSharp.LogError($"Disconnected failed: player index {clientIndex} not found");
            return;
        }

        player.Disconnect();

        _playerListener.OnDisconnected(player);

        _players.Remove(player);
    }

    public void UpdatePlayerName(int clientIndex, string playerName)
    {
        var player = _players.SingleOrDefault(x => x.Index == clientIndex);
        if (player is null)
        {
            _sourceSharp.LogError($"UpdateName failed: player index {clientIndex} not found");
            return;
        }

        // update name cache
        player.ChangeName(playerName);
    }

    /*
     * Internal
     */
    private void RunAdminCheck(CGamePlayer player, bool call)
    {
        player.InvalidateAdmin();

        var adminUser = _adminManager.FindAdminByIdentity(player.SteamId);
        if (adminUser is CAdmin admin)
        {
            player.AdminCheck(admin.Flags);
        }

        if (call)
        {
            _playerListener.OnPostAdminCheck(player);
        }
    }

    /*
     * IPlayerManager
     */
    public uint GetNumPlayers() => (uint)_players.Count;
    public uint GetMaxClients() => (uint)_players.Capacity;
    public IReadOnlyList<GamePlayer> GetClients() => _players.AsReadOnly();
    public IReadOnlyList<GamePlayer> GetPlayers() => _players.FindAll(x => x is { IsSourceTv: false, IsReplay: false }).AsReadOnly();
    public GamePlayer? GetGamePlayerByUserId(int userId) => _players.SingleOrDefault(x => x.UserId == userId);
    public GamePlayer? GetGamePlayer(uint serial) => _players.SingleOrDefault(x => x.Serial == serial);
    public GamePlayer? GetGamePlayer(int index) => _players.SingleOrDefault(x => x.Index == index);
    public void RunAdminChecks() => _players.ForEach(p => RunAdminCheck(p, false));

    /*
     *  IRuntime
     */
    public string GetInterfaceName() => SharedDefines.PlayerManagerInterfaceName;
    public uint GetInterfaceVersion() => SharedDefines.PlayerManagerInterfaceVersion;
}
