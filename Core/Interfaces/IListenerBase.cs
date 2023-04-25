using SourceSharp.Core.Models;

namespace SourceSharp.Core.Interfaces;

internal interface IListenerBase : IModuleBase
{
    void OnPluginLoad(CPlugin plugin);
    void OnPluginUnload(CPlugin plugin);
}
