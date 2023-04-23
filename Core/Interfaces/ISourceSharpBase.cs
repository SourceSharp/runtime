using SourceSharp.Sdk;
using SourceSharp.Sdk.Enums;
using SourceSharp.Sdk.Interfaces;
using System;
using System.Reflection;

namespace SourceSharp.Core.Interfaces;

internal interface ISourceSharpBase : ISourceSharp
{
    /// <summary>
    /// 运行帧
    /// </summary>
    void RunFrame(int tickCount, float gameTime);
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

    public virtual int GetGameTickCount(bool readFromPtr = false)
    {
        throw new NotImplementedException();
    }

    public virtual int GetGameTime(bool readFromPtr = false)
    {
        throw new NotImplementedException();
    }

    /*
     * ISourceSharpBase
     */

    public virtual void RunFrame(int tickCount, float gameTime)
    {
        throw new NotImplementedException();
    }
}
