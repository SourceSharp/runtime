using SourceSharp.Sdk.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceSharp.Core.Modules;

internal class ShareSystem : IShareSystem
{
    private class SharedInterface
    {
#nullable disable
        public IRuntime @Interface { get; }
        public IPlugin @Plugin { get; }
#nullable restore

        public SharedInterface(IRuntime @interface, IPlugin @plugin)
        {
            @Interface = @interface;
            @Plugin = @plugin;
        }
    }

    private readonly List<SharedInterface> _interfaces;

    public ShareSystem()
    {
        _interfaces = new();
    }

    public void AddInterface(IRuntime @interface, IPlugin @plugin)
    {
        var name = @interface.GetInterfaceName();

        if (_interfaces.Any(x => x.Interface.GetInterfaceName() == name))
        {
            throw new InvalidOperationException($"Interface with name {name} already exists!");
        }

        _interfaces.Add(new(@interface, @plugin));
    }

    public T GetRequiredInterface<T>(uint version) where T : IRuntime
    {
        var @interface = _interfaces.SingleOrDefault(x => x.GetType() == typeof(T)) ??
                         throw new NotImplementedException($"Interface <{nameof(T)}> not found.");

        if (@interface.Interface.GetInterfaceVersion() < version)
        {
            throw new NotImplementedException($"Interface <{nameof(T)}> version is lower.");
        }

        return (T)@interface.Interface;
    }

    public T? GetInterface<T>(uint version) where T : IRuntime
        => (T?)_interfaces.SingleOrDefault(x => x.GetType() == typeof(T) && x.Interface.GetInterfaceVersion() >= version)?.Interface;
}
