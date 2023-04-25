using SourceSharp.Sdk.Enums;
using SourceSharp.Sdk.Models;

namespace SourceSharp.Core.Models;

internal sealed class CAdmin : AdminUser
{
    private readonly uint _id;
    private readonly string _name;
    private readonly ulong _steamId;
    private AdminFlags _flags;

    public CAdmin(ulong steamId, string name, uint serial)
    {
        _steamId = steamId;
        _name = name;
        _id = serial;
        _flags = AdminFlags.None;
    }

    /*
     *  Impl
     */
    protected override uint GetId() => _id;
    protected override string GetName() => _name;
    protected override ulong GetSteamId() => _steamId;
    protected override AdminFlags GetFlags() => _flags;

    public override void SetAdminFlags(AdminFlags flags) => _flags = flags;
    public override void AddAdminFlags(AdminFlags flags) => _flags |= flags;
    public override void RemoveAdminFlags(AdminFlags flags) => _flags &= ~flags;
    public override bool HasFlags(AdminFlags flags) => _flags.HasFlag(flags);
}
