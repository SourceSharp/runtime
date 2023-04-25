using SourceSharp.Sdk.Interfaces;

namespace SourceSharp.Core.Interfaces;

internal interface IModuleBase : IRuntime
{
    void Initialize();
    void Shutdown();
}
