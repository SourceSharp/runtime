namespace SourceSharp.Sdk.Interfaces;

public interface IPlugin
{
    bool OnLoad();
    bool QueryRunning();
    void OnShutdown();
}