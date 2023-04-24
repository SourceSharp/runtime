using SourceSharp.Sdk.Interfaces;
using System.Net;

namespace SourceSharp.Core.Interfaces;

internal interface IPlayerManagerBase : IPlayerManager
{
    void OnConnected(int clientIndex, int userId, ulong steamId, string name, IPEndPoint remoteEndpoint);
    void OnAuthorized(int clientIndex, ulong steamId);
    void OnPutInServer(int clientIndex, bool fakeClient, bool sourceTv, bool replay);
    void OnDisconnecting(int clientIndex);
    void OnDisconnected(int clientIndex);
    void UpdatePlayerName(int clientIndex, string playerName);
}
