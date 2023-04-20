using SourceSharp.Sdk.Attributes;
using SourceSharp.Sdk.Interfaces;
using SourceSharp.Sdk.Models;
using System.Reflection;

namespace SourceSharp.Example;

public interface IExportInterface : IRuntime
{
    void TestExport();
}

[Plugin(Name = "Example", Author = "SourceSharp Team", Version = "1.0")]
public class Example : PluginBase, IExportInterface
{
    public override bool OnLoad()
    {
        _sourceSharp.LogMessage("plugin loaded");
        _shareSystem.AddInterface(this, this);
        return true;
    }

    public override void OnAllLoad()
    {
        var export = _shareSystem.GetRequiredInterface<IExportInterface>(1);
        export.TestExport();
    }

    public override void OnShutdown()
        => _sourceSharp.LogMessage("plugin unloaded");

    [ServerConsoleCommand(Command = "ss_test")]
    private void TestCommand(ConsoleCommand command)
        => _sourceSharp.LogMessage("test command executed: " + command.ArgString);

    /*
     * Export
     */
    public void TestExport() => _sourceSharp.LogMessage("Test Export");
    public string GetInterfaceName() => "ISOURCESHARP_" + Assembly.GetExecutingAssembly()!.GetName().Name!.ToUpper();
    public uint GetInterfaceVersion() => (uint)Assembly.GetExecutingAssembly()!.GetName()!.Version!.Major;
}