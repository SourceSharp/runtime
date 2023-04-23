using SourceSharp.Core.Models;
using SourceSharp.Sdk.Interfaces;

namespace SourceSharp.Core.Interfaces;

internal interface IAdminManagerBase : IAdminManager
{
    void CheckGameUserAdmin(CGamePlayer player);
}
