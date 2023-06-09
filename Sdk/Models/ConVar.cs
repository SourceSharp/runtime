using SourceSharp.Sdk.Enums;
using SourceSharp.Sdk.Interfaces;
using SourceSharp.Sdk.Structs;
using System;

namespace SourceSharp.Sdk.Models;

public abstract class ConVar
{
    /// <summary>
    /// 最大值/最小值
    /// </summary>
    public ConVarBounds Bounds
    {
        get => GetBounds();
        set => SetBounds(value);
    }

    /// <summary>
    /// Flags
    /// </summary>
    public ConVarFlags Flags => GetFlags();

    /// <summary>
    /// 新增Flags
    /// </summary>
    /// <param name="flags"><see cref="ConVarFlags"/>flags</param>
    public abstract void AddFlags(ConVarFlags flags);

    /// <summary>
    /// 名字
    /// </summary>
    public string Name => GetName();

    /// <summary>
    /// 默认值
    /// </summary>
    public string DefaultValue => GetDefaultValue();

    /// <summary>
    /// 说明文字
    /// </summary>
    public string Description => GetDescription();

    /// <summary>
    /// 读取指定类型的值
    /// </summary>
    /// <typeparam name="T">bool, int, float, string</typeparam>
    /// <returns>值</returns>
    public abstract T Get<T>() where T : IConvertible;

    /// <summary>
    /// 设置指定类型的值
    /// </summary>
    /// <typeparam name="T">bool, int, float, string</typeparam>
    /// <param name="value"></param>
    public abstract void Set<T>(T value) where T : IConvertible;

    /// <summary>
    /// 不修改实际值的情况下, 修改客户端中的值
    /// </summary>
    /// <param name="players">玩家</param>
    /// <returns>True = 成功</returns>
    public abstract bool ReplicateToPlayers(GamePlayer[] players);

    /// <summary>
    /// 订阅修改事件
    /// </summary>
    public abstract void RegisterChangeHook(IPlugin caller, Action<ConVar, string, string> callback);

    protected abstract ConVarBounds GetBounds();
    protected abstract void SetBounds(ConVarBounds bounds);
    protected abstract string GetDescription();
    protected abstract string GetDefaultValue();
    protected abstract ConVarFlags GetFlags();
    protected abstract string GetName();
}
