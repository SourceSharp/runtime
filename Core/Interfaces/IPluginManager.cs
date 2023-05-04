using System;

namespace SourceSharp.Core.Interfaces;

internal interface IPluginManager
{
    void Initialize(IServiceProvider services);
    void Shutdown();
    void Signal();

    // invoker
    void OnGameFrame(bool simulating);
}
