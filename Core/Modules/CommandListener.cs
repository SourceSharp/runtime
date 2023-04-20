using SourceSharp.Core.Interfaces;
using SourceSharp.Core.Models;
using SourceSharp.Sdk;
using System.Collections.Generic;

namespace SourceSharp.Core.Modules;

internal class CommandListener : ICommandListener
{
    public CommandListener()
    {

    }

    public void Initialize(List<SourceSharpPlugin> plugins)
    {
        throw new System.NotImplementedException();
    }

    public void Shutdown()
    {
        throw new System.NotImplementedException();
    }

    public void OnPluginUnload(SourceSharpPlugin plugin)
    {
        throw new System.NotImplementedException();
    }

    /*
     *  IRuntime
     */
    public string GetInterfaceName() => SharedDefines.CommandListenerInterfaceName;
    public uint GetInterfaceVersion() => SharedDefines.CommandListenerInterfaceVersion;
}
