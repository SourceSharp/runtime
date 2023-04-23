using SourceSharp.Core.Models;
using SourceSharp.Sdk.Interfaces;
using System.Collections.Generic;

namespace SourceSharp.Core.Interfaces;

internal interface IModuleBase : IRuntime
{
    void Initialize(List<SourceSharpPlugin> plugins);
    void Shutdown();

    void OnPluginUnload(SourceSharpPlugin plugin);
}
