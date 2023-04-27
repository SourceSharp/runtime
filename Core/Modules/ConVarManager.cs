using SourceSharp.Core.Bridges;
using SourceSharp.Core.Interfaces;
using SourceSharp.Core.Models;
using SourceSharp.Core.Utils;
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
    private sealed class ConVarHook : CHookCallback<Action<ConVar, string, string>>
    {
        public CConVar ConVar { get; }

        internal ConVarHook(CPlugin plugin, CConVar conVar, MethodInfo method) : base(plugin, method)
        {
            ConVar = conVar;
        }
    }

    private readonly Dictionary<string, List<ConVarHook>> _hooks;
    private readonly List<CConVar> _conVars;

    private readonly ISourceSharpBase _sourceSharp;

    public ConVarManager(ISourceSharpBase sourceSharp)
    {
        _sourceSharp = sourceSharp;

        _hooks = new();
        _conVars = new();
    }

    public void Initialize()
    {
        // do nothing
    }

    public void Shutdown()
    {
        _conVars.Clear();
        _hooks.Clear();
    }

    public void OnPluginLoad(CPlugin plugin)
    {
        RegConVars(plugin);
        HookConVars(plugin);
    }

    public void OnPluginUnload(CPlugin plugin)
    {
        foreach (var (conVarName, hooks) in _hooks.Where(x => x.Value.Any(v => v.Plugin == plugin)))
        {
            for (var i = 0; i < hooks.Count; i++)
            {
                if (hooks[i].Plugin.Equals(plugin))
                {
                    hooks.RemoveAt(i);
                    i--;
                }
            }

            if (!hooks.Any())
            {
                _hooks.Remove(conVarName);
                break;
            }
        }
    }

    public void OnConVarChanged(IConVar conVar, string oldValue, string newValue)
    {
        if (!_hooks.TryGetValue(conVar.Name, out var hooks))
        {
            return;
        }

        foreach (var hook in hooks)
        {
            hook.Callback.Invoke(hook.ConVar, oldValue, newValue);
        }
    }

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
    {
        var hooks = plugin.Instance.GetType().GetMethods()
            .Where(m => Attribute.GetCustomAttributes(m, typeof(ConVarChangedAttribute)).Any())
            .ToList();

        if (!hooks.Any())
        {
            return;
        }

        foreach (var hook in hooks)
        {
            if (Attribute.GetCustomAttribute(hook, typeof(ConVarChangedAttribute)) is not ConVarChangedAttribute cvar)
            {
                continue;
            }

            hook.CheckReturnAndParameters(typeof(void), new[] { typeof(ConVar), typeof(string), typeof(string) });

            var conVar = _conVars.Find(x => x.Name == cvar.Name);
            if (conVar is null)
            {
                // lookup game ConVar
                var iCvar = ConVarBridge.FindConVar(cvar.Name);
                if (iCvar is null)
                {
                    _sourceSharp.LogError($"Failed to find ConVar: {cvar.Name}");
                    plugin.UpdateStatus(PluginStatus.Error);
                    return;
                }

                conVar = new CConVar(iCvar, iCvar.Name, iCvar.Description);
                _conVars.Add(conVar);
            }

            if (!_hooks.ContainsKey(cvar.Name))
            {
                _hooks.Add(cvar.Name, new());
            }

            _hooks[cvar.Name].Add(new(plugin, conVar, hook));
        }
    }

    /*
     *  IRuntime
     */
    public string GetInterfaceName() => SharedDefines.ConVarManagerInterfaceName;
    public uint GetInterfaceVersion() => SharedDefines.ConVarManagerInterfaceVersion;

}
