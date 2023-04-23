using SourceSharp.Core.Models;
using SourceSharp.Sdk.Interfaces;

namespace SourceSharp.Core.Interfaces;

internal interface IModuleBase : IRuntime
{
    void Initialize();
    void Shutdown();

    void OnPluginLoad(CPlugin plugin);
    void OnPluginUnload(CPlugin plugin);
}
