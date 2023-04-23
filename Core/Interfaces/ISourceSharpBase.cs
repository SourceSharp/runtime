using SourceSharp.Sdk;
using SourceSharp.Sdk.Enums;
using SourceSharp.Sdk.Interfaces;
using System;
using System.Reflection;

namespace SourceSharp.Core.Interfaces;

internal interface ISourceSharpBase : ISourceSharp
{

}

internal abstract class SourceSharpBase : ISourceSharpBase
{
    /*
     *  IRuntime
     */
    public string GetInterfaceName() => SharedDefines.CoreInterfaceName;
    public uint GetInterfaceVersion() => (uint)Assembly.GetExecutingAssembly()!.GetName()!.Version!.Major;

    /*
     * ISourceSharp
     */
    public virtual void Invoke(Action action)
    {
        throw new NotImplementedException();
    }

    public virtual void InvokeNextFrame(Action action)
    {
        throw new NotImplementedException();
    }

    public virtual string BuildPath(PathType type, params string[] format)
    {
        throw new NotImplementedException();
    }

    public virtual string GetGamePath()
    {
        throw new NotImplementedException();
    }

    public virtual string GetSourceSharpPath()
    {
        throw new NotImplementedException();
    }

    public virtual void LogError(string message)
    {
        throw new NotImplementedException();
    }

    public virtual void LogMessage(string message)
    {
        throw new NotImplementedException();
    }

    public virtual void PrintLine(string message)
    {
        throw new NotImplementedException();
    }

}
