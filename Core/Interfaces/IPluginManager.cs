namespace SourceSharp.Core.Interfaces;

internal interface IPluginManager
{
    void Initialize();
    void Shutdown();

    void Signal();
}
