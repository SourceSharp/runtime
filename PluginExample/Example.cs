using SourceSharp.Sdk;
using SourceSharp.Sdk.Attributes;
using SourceSharp.Sdk.Enums;
using SourceSharp.Sdk.Interfaces;
using SourceSharp.Sdk.Models;
using SourceSharp.Sdk.Structs;
using System.Reflection;

namespace SourceSharp.Example;

#pragma warning disable IDE0051, IDE0052, IDE0060, IDE1006

public interface IExportInterface : IRuntime
{
    void TestExport();
}

[Plugin(Name = "Example", Author = "SourceSharp Team", Version = "1.0")]
public class Example : PluginBase, IExportInterface
{
    [ConVar("ss_plugin_example", "0")]
    public ConVar ss_plugin_example { get; } = null!;

    [ConVar("test_convar", "1",
        Description = "测试",
        Flags = ConVarFlags.Notify | ConVarFlags.Replicated,
        HasMin = true, Min = 0f, HasMax = true, Max = 999f)]
    public ConVar test_convar { get; } = null!;

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

    [ServerConsoleCommand("ss_s_test", "测试命令")]
    private void TestServerCommand(ConsoleCommand command)
        => _sourceSharp.LogMessage("test command executed: " + command.ArgString);

    [ClientConsoleCommand("ss_c_test", "测试命令", ConVarFlags.Release | ConVarFlags.ServerCanExecute, AdminFlags.ChangeMap)]
    private void TestClientCommand(ConsoleCommand command, GamePlayer? player)
        => _sourceSharp.LogMessage("test command executed: " + command.ArgString);

    [GameEvent("player_spawn")]
    private ActionResponse<bool> OnPlayerSpawn(GameEvent @event)
    {
        _sourceSharp.LogMessage("player spawned -> userId: " + @event.Get<int>("userid"));
        return default;
    }

    [GameFrame]
    private void OnGameFrame(bool simulating)
        => _sourceSharp.LogMessage($"OnGameFrame({simulating})");

    [PlayerListener(PlayerListenerType.Connected)]
    private void OnPlayerConnected(GamePlayer player)
        => _sourceSharp.LogMessage("player connected -> name: " + player.Name);

    [ConVarChanged("sv_cheats")]
    [ConVarChanged("mp_allow_bot")]
    public void OnConVarChanged(ConVar conVar, string oldValue, string newValue)
        => _sourceSharp.LogMessage($"ConVarChanged -> {conVar.Name} = {newValue} (old: {oldValue})");

    /*
     * Export
     */
    public void TestExport() => _sourceSharp.LogMessage("Test Export");
    public string GetInterfaceName() => SharedDefines.InterfacePrefix + Assembly.GetExecutingAssembly()!.GetName().Name!.ToUpper();
    public uint GetInterfaceVersion() => (uint)Assembly.GetExecutingAssembly()!.GetName()!.Version!.Major;
}