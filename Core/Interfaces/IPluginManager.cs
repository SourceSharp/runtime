using SourceSharp.Core.Models;
using SourceSharp.Sdk.Interfaces;
using System;

namespace SourceSharp.Core.Interfaces;

internal interface IPluginManager
{
    void Initialize(IServiceProvider services);
    void Shutdown();
    void Signal();

    // invoker
    void OnGameFrame(bool simulating);

    // native
    CPlugin? FindPlugin(IPlugin plugin);
}
