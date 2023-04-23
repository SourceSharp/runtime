using SourceSharp.Sdk.Interfaces;
using System;
using System.Collections.Generic;

namespace SourceSharp.Core.Interfaces;

internal interface IShareSystemBase : IShareSystem
{
    List<IRuntime> CheckUnloadPluginInterfaces(IPlugin plugin);
}

public abstract class ShareSystemBase : IShareSystemBase
{
    public virtual void AddInterface(IRuntime @interface, IPlugin @plugin)
    {
        throw new NotImplementedException();
    }

    public virtual T GetRequiredInterface<T>(uint version) where T : class, IRuntime
    {
        throw new NotImplementedException();
    }

    public virtual T? GetInterface<T>(uint version) where T : class, IRuntime
    {
        throw new NotImplementedException();
    }

    public virtual List<IRuntime> CheckUnloadPluginInterfaces(IPlugin plugin)
    {
        throw new NotImplementedException();
    }
}