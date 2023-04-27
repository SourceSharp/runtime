using SourceSharp.Core.Models;
using SourceSharp.Sdk;
using SourceSharp.Sdk.Enums;
using SourceSharp.Sdk.Interfaces;
using SourceSharp.Sdk.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ConVarBridge = SourceSharp.Core.Bridges.ConVar;
using CoreBridge = SourceSharp.Core.Bridges.SourceSharp;
using EventBridge = SourceSharp.Core.Bridges.Event;

namespace SourceSharp.Core.Interfaces;

internal interface ISourceSharpBase : ISourceSharp
{
    /// <summary>
    /// 运行帧
    /// </summary>
    void RunFrame();
}

internal abstract class SourceSharpBase : ISourceSharpBase
{
    /*
     *  IRuntime
     */
    public string GetInterfaceName() => SharedDefines.CoreInterfaceName;
    private readonly uint _versionNumber = (uint)Assembly.GetExecutingAssembly()!.GetName()!.Version!.Major;
    public uint GetInterfaceVersion() => _versionNumber;

    /*
     * ISourceSharp
     */
    public abstract void Invoke(Action action);
    public abstract void InvokeNextFrame(Action action);

    public string BuildPath(PathType type, params string[] format)
    {
        if (!format.Any())
        {
            throw new ArgumentException("Invalid format path!");
        }

        string finalPath;
        switch (type)
        {
            case PathType.SourceSharpAbsolute:
                {
                    var ssDir = GetSourceSharpPath();
                    finalPath = ssDir;
                    foreach (var p in format)
                    {
                        finalPath = Path.Combine(finalPath, p);
                    }

                    break;
                }
            case PathType.Game:
                {
                    var gameDir = GetGamePath();
                    finalPath = gameDir;
                    foreach (var p in format)
                    {
                        finalPath = Path.Combine(finalPath, p);
                    }

                    break;
                }
            case PathType.None:
                {
                    finalPath = format[0];
                    for (var i = 1; i < format.Length; i++)
                    {
                        finalPath = Path.Combine(finalPath, format[i]);
                    }

                    break;
                }
            default:
                {
                    throw new ArgumentException("Invalid PathType given.");
                }
        }

        return finalPath;
    }

    public string GetGamePath()
        => CoreBridge.GetGamePath();

    public string GetSourceSharpPath()
        => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

    public void LogError(string message)
    {
        PrintError(message);
        CoreBridge.LogError(message);
    }

    public void LogMessage(string message)
    {
        PrintLog(message);
        CoreBridge.LogMessage(message);
    }

    public void PrintLine(string message)
        => Console.WriteLine(message);

    public virtual int GetMaxClients()
        => CoreBridge.GetMaxClients();

    public virtual int GetMaxHumanPlayers()
        => CoreBridge.GetMaxHumanPlayers();

    public GameEngineVersion GetEngineVersion()
        => (GameEngineVersion)CoreBridge.GetEngineVersion();

    public void ExecuteServerCommand(string command)
        => CoreBridge.ExecuteServerCommand(command);

    public void InsertServerCommand(string command)
        => CoreBridge.InsertServerCommand(command);

    public void ServerExecute()
        => CoreBridge.ServerExecute();

    public ConVar FindConVar(string name)
    {
        var iCvar = ConVarBridge.FindConVar(name);
        return new CConVar(iCvar, iCvar.Name, iCvar.Description);
    }

    public GameEvent CreateEvent(string name, bool broadcast)
    {
        var ev = EventBridge.CreateGameEvent(name, broadcast);
        return new CGameEvent(ev, false);
    }

    /*
     * ISourceSharpBase
     */

    public abstract void RunFrame();

    protected virtual void PrintError(string message)
    {
        var color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("[Fail] ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"{DateTime.Now:yyyy/MM/dd HH:mm:ss} [SourceSharp] ");
        Console.ForegroundColor = color;
        Console.WriteLine(message);
    }

    protected virtual void PrintLog(string message)
    {
        var color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("[Info] ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"{DateTime.Now:yyyy/MM/dd HH:mm:ss} [SourceSharp] ");
        Console.ForegroundColor = color;
        Console.WriteLine(message);
    }
}
