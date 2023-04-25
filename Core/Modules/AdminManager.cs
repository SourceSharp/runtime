using SourceSharp.Core.Interfaces;
using SourceSharp.Core.Models;
using SourceSharp.Sdk;
using SourceSharp.Sdk.Interfaces;
using SourceSharp.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceSharp.Core.Modules;

internal sealed class AdminManager : IAdminManager
{
    private readonly ISourceSharpBase _sourceSharp;

    private readonly List<CAdmin> _admins;
    private uint _serialNumber;

    public AdminManager(ISourceSharpBase sourceSharp)
    {
        _sourceSharp = sourceSharp;

        _admins = new();
        _serialNumber = 0;
    }

    /*
     * AdminManager
     */
    public AdminUser CreateAdmin(ulong steamId, string name)
    {
        if (_admins.Any(x => x.SteamId == steamId))
        {
            throw new InvalidOperationException($"SteamId {steamId} is already admin!");
        }

        var admin = new CAdmin(steamId, name, ++_serialNumber);
        _admins.Add(admin);

        return admin;
    }

    public void RemoveAdmin(AdminUser adminUser)
    {
        if (adminUser is not CAdmin admin)
        {
            throw new ArgumentException("instance is not admin");
        }

        if (!_admins.Contains(admin))
        {
            throw new InvalidOperationException("instance is not admin");
        }

        _admins.Remove(admin);
    }

    public IReadOnlyList<AdminUser> GetAdmins() => _admins.AsReadOnly();

    public AdminUser? FindAdminByIdentity(ulong steamId) => _admins.SingleOrDefault(x => x.SteamId == steamId);

    /*
     *  IRuntime
     */
    public string GetInterfaceName() => SharedDefines.AdminManagerInterfaceName;
    public uint GetInterfaceVersion() => SharedDefines.AdminManagerInterfaceVersion;
}
