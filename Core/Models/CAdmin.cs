using SourceSharp.Sdk.Models;

namespace SourceSharp.Core.Models;

internal sealed class CAdmin : AdminUser
{
    public CAdmin(ulong steamId, string name, uint serial)
    {
        SteamId = steamId;
        Name = name;
        Id = serial;
    }
}
