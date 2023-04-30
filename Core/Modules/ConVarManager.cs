using SourceSharp.Core.Bridges;
using SourceSharp.Core.Interfaces;
using SourceSharp.Core.Models;
using SourceSharp.Sdk;
using SourceSharp.Sdk.Attributes;
using SourceSharp.Sdk.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ConVar = SourceSharp.Sdk.Models.ConVar;
using ConVarBridge = SourceSharp.Core.Bridges.ConVar;

namespace SourceSharp.Core.Modules;

internal class ConVarManager : IConVarManager
{
    private readonly struct ConVarHookInfo
    {
        public CConVar ConVar { get; }
        public ConVarHookInfo(CConVar cvar) => ConVar = cvar;
    }

    private sealed class ConVarHook : CHookCallback<Action<ConVar, string, string>>
    {
        public CConVar ConVar { get; }

        internal ConVarHook(CPlugin plugin, CConVar conVar, MethodInfo method) : base(plugin, method)
        {
            ConVar = conVar;
        }
    }

    private readonly CKeyHook<string,
        ConVarHookInfo,
        ConVarChangedAttribute,
        bool,
        Action<ConVar, string, string>> _hooks;

    private readonly List<CConVar> _conVars;

    private readonly ISourceSharpBase _sourceSharp;

    public ConVarManager(ISourceSharpBase sourceSharp)
    {
        _sourceSharp = sourceSharp;

        _conVars = new();
        _hooks = new();
    }

    public void Initialize()
    {
        // do nothing
    }

    public void Shutdown()
    {
        _conVars.Clear();
        _hooks.Shutdown();
    }

    public void OnPluginLoad(CPlugin plugin)
    {
        RegConVars(plugin);
        HookConVars(plugin);
    }

    public void OnPluginUnload(CPlugin plugin)
        => _hooks.RemovePlugin(plugin);

    public void OnConVarChanged(SSConVar conVar, string oldValue, string newValue)
        => _hooks.OnCall(conVar.Name, false, hooks =>
        {
            foreach (var hook in hooks)
            {
                hook.Callback.Invoke(hook.Info.ConVar, oldValue, newValue);
            }

            return true;
        });

    private void RegConVars(CPlugin plugin)
    {
        var conVars = plugin.Instance.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => Attribute.GetCustomAttributes(m, typeof(ConVarAttribute)).Any())
            .ToList();

        if (!conVars.Any())
        {
            return;
        }

        foreach (var cvarAttr in conVars)
        {
            if (Attribute.GetCustomAttribute(cvarAttr, typeof(ConVarAttribute)) is not ConVarAttribute cvar)
            {
                continue;
            }

            var iCvar = ConVarBridge.CreateConVar(cvar.Name, cvar.DefaultValue,
                cvar.Description, (int)cvar.Flags,
                cvar.HasMin, cvar.Min, cvar.HasMax, cvar.Max);

            if (iCvar is null)
            {
                _sourceSharp.LogError($"Failed to create ConVar: {cvar.Name}");
                plugin.UpdateStatus(PluginStatus.Error);
                return;
            }

            var conVar = new CConVar(iCvar, iCvar.Name, iCvar.Description);
            _conVars.Add(conVar);
        }
    }

    private void HookConVars(CPlugin plugin)
        => _hooks.ScanPlugin(plugin,
            attr =>
            {
                var conVar = _conVars.Find(x => x.Name == attr.Name);
                if (conVar is null)
                {
                    // lookup game ConVar
                    var iCvar = ConVarBridge.FindConVar(attr.Name) ??
                                throw new InvalidOperationException(
                                    $"Failed to find ConVar: [{attr.Name}], make sure ConVar does exists.");

                    conVar = new CConVar(iCvar, iCvar.Name, iCvar.Description);
                    _conVars.Add(conVar);
                }

                return new ConVarHookInfo(conVar);
            },
            ConVarBridge.RegisterConVarHook);

    /*
     *  IRuntime
     */
    public string GetInterfaceName() => SharedDefines.ConVarManagerInterfaceName;
    public uint GetInterfaceVersion() => SharedDefines.ConVarManagerInterfaceVersion;

}
