using SourceSharp.Sdk.Attributes;
using SourceSharp.Sdk.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SourceSharp.Core.Models;

/// <summary>
/// 使用Key的Hook类型
/// </summary>
/// <typeparam name="TKey">Hook Key</typeparam>
/// <typeparam name="TInfo">Hook中保存的相关信息</typeparam>
/// <typeparam name="TAttribute">要扫描的Attribute</typeparam>
/// <typeparam name="TCallReturn">Hook调用时的返回值类型</typeparam>
/// <typeparam name="TCallback">Callback</typeparam>
internal class CKeyHook<TKey, TInfo, TAttribute, TCallReturn, TCallback>
    where TKey : IConvertible
    where TInfo : struct
    where TAttribute : HookBaseAttribute<TKey>
    where TCallback : Delegate
{
    internal sealed class ParamsHook : CHookCallback<TCallback>
    {
        public TInfo Info { get; }

        internal ParamsHook(CPlugin plugin, TInfo info, MethodInfo method) : base(plugin, method)
        {
            Info = info;
        }
    }

    private readonly Dictionary<TKey, List<ParamsHook>> _hooks;

    public CKeyHook() => _hooks = new();

    /// <summary>
    /// 清理Hook
    /// </summary>
    public void Shutdown() => _hooks.Clear();

    /// <summary>
    /// 扫描插件是否使用Hook
    /// </summary>
    /// <param name="plugin">CPlugin实例</param>
    /// <param name="convertInfo">转换Attribute数据到Info实体的Lambda方法</param>
    /// <param name="register">注册机</param>
    public void ScanPlugin(CPlugin plugin, Func<TAttribute, TInfo> convertInfo, Action<TKey>? register)
    {
        var hooks = plugin.Instance.GetType()
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
            .Where(m => m.GetCustomAttributes(typeof(TAttribute), false).Any())
            .ToList();

        if (!hooks.Any())
        {
            return;
        }

        foreach (var hook in hooks)
        {
            if (Attribute.GetCustomAttribute(hook, typeof(TAttribute)) is not TAttribute t)
            {
                continue;
            }

            if (Subscribe(t.Key, plugin, convertInfo(t), hook))
            {
                register?.Invoke(t.Key);
            }
        }
    }

    /// <summary>
    /// 订阅Hook
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="plugin">CPlugin实例</param>
    /// <param name="info">HookInfo</param>
    /// <param name="method">方法</param>
    /// <returns></returns>
    private bool Subscribe(TKey key, CPlugin plugin, TInfo info, MethodInfo method)
    {
        var create = false;
        if (!_hooks.ContainsKey(key))
        {
            _hooks[key] = new();
            create = true;
        }

        _hooks[key].Add(new(plugin, info, method));
        return create;
    }

    /// <summary>
    /// 注销插件
    /// </summary>
    /// <param name="plugin"></param>
    public void RemovePlugin(CPlugin plugin)
    {
        foreach (var (key, hooks) in _hooks.Where(x => x.Value.Any(v => v.Plugin == plugin)))
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
                _hooks.Remove(key);
                break;
            }
        }
    }

    /// <summary>
    /// Hook调用时
    /// </summary>
    /// <param name="key">Hook Key</param>
    /// <param name="defaultReturn">默认返回值 (在没有相关Hook时直接返回)</param>
    /// <param name="iterator">迭代器</param>
    /// <returns>迭代器中的返回值</returns>
    public TCallReturn OnCall(TKey key, TCallReturn defaultReturn, Func<IEnumerable<ParamsHook>, TCallReturn> iterator)
        => !_hooks.TryGetValue(key, out var hooks) || !hooks.Any(x => x.Plugin.Status is PluginStatus.Running)
            ? defaultReturn
            : iterator.Invoke(hooks.Where(x => x.Plugin.Status is PluginStatus.Running));
}
