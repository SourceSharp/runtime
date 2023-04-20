using SourceSharp.Sdk.Attributes;
using SourceSharp.Sdk.Interfaces;
using SourceSharp.Sdk.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace SourceSharp.Example;

[Plugin(Name = "Example", Author = "SourceSharp Team", Version = "1.0")]
public class Example : IPlugin
{
    private readonly ICore _core;

    public Example(ICore core) => _core = core;

    public bool OnLoad()
    {
        _core.LogMessage("plugin loaded");
        return true;
    }

    public bool QueryRunning() => true;

    public void OnShutdown()
    {
        _core.LogMessage("plugin unloaded");
    }

    [ServerConsoleCommand(Command = "ss_test")]
    private void TestCommand(ConsoleCommand command)
    {
        _core.LogMessage("test command executed: " + command.ArgString);
    }

    class GamePlayer
    {

    }
    class WarcraftPlayer
    {
        public int Exp;
    }

    private void OnClientConnected(GamePlayer player)
    {
        var w3p = new WarcraftPlayer();

        Task.Run(async () =>
        {
            var http = new HttpClient();

            await Task.CompletedTask;
            var response = new { Exp = 100 };

            // or?
            lock (w3p)
            {
                w3p.Exp = response.Exp;
            }

            // or?
            _core.Invoke(() =>
            {
                w3p.Exp = response.Exp;
            });
        });
    }
}