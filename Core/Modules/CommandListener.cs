using SourceSharp.Core.Interfaces;
using SourceSharp.Core.Models;
using SourceSharp.Sdk;
using System;

namespace SourceSharp.Core.Modules;

internal class CommandListener : ICommandListener
{
    public CommandListener()
    {

    }

    public void Initialize()
    {
        throw new NotImplementedException();
    }

    public void Shutdown()
    {
        throw new NotImplementedException();
    }

    public void OnPluginLoad(CPlugin plugin)
    {
        throw new NotImplementedException();
    }

    public void OnPluginUnload(CPlugin plugin)
    {
        throw new NotImplementedException();
    }

    /*
     *  IRuntime
     */
    public string GetInterfaceName() => SharedDefines.CommandListenerInterfaceName;
    public uint GetInterfaceVersion() => SharedDefines.CommandListenerInterfaceVersion;
}
