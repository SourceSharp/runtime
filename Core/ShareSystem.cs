using SourceSharp.Core.Interfaces;
using SourceSharp.Sdk.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceSharp.Core;

internal sealed class ShareSystem : ShareSystemBase
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

    public override void AddInterface(IRuntime @interface, IPlugin @plugin)
    {
        var name = @interface.GetInterfaceName();

        if (_interfaces.Any(x => x.Interface.GetInterfaceName() == name))
        {
            throw new InvalidOperationException($"Interface with name {name} already exists!");
        }

        _interfaces.Add(new(@interface, @plugin));
    }

    public override T GetRequiredInterface<T>(uint version)
    {
        var @interface = _interfaces.SingleOrDefault(x => x.GetType() == typeof(T)) ??
                         throw new NotImplementedException($"Interface <{nameof(T)}> not found.");

        if (@interface.Interface.GetInterfaceVersion() < version)
        {
            throw new NotImplementedException($"Interface <{nameof(T)}> version is lower.");
        }

        return (T)@interface.Interface;
    }

    public override T? GetInterface<T>(uint version) where T : class
        => (T?)_interfaces.SingleOrDefault(x => x.GetType() == typeof(T) && x.Interface.GetInterfaceVersion() >= version)?.Interface;

    public override List<IRuntime> CheckUnloadPluginInterfaces(IPlugin plugin)
    {
        var interfaces = _interfaces.Where(x => x.Plugin == plugin).ToList();
        _interfaces.RemoveAll(x => x.Plugin == plugin);
        return interfaces.Select(x => x.Interface).ToList();
    }
}
