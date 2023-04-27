using System;

namespace SourceSharp.Sdk.Models;

// TODO CreateNew, Fire, FireToPlayers, Cancel

public abstract class GameEvent
{
    /// <summary>
    /// Event 名字
    /// </summary>
    public string Name => GetName();

    public bool Broadcast => GetBroadcast();

    /// <summary>
    /// 读取Event的值
    /// </summary>
    /// <typeparam name="T">bool, int, float, string</typeparam>
    /// <param name="key">字段名</param>
    /// <returns>值</returns>
    public abstract T Get<T>(string key) where T : IConvertible;

    /// <summary>
    /// 设置Event的值
    /// </summary>
    /// <typeparam name="T">bool, int, float, string</typeparam>
    /// <param name="key">字段名</param>
    /// <param name="value"></param>
    /// <returns>True = 成功</returns>
    public abstract bool Set<T>(string key, T value) where T : IConvertible;

    protected abstract string GetName();
    protected abstract void SetBroadcast(bool broadcast);
    protected abstract bool GetBroadcast();
}
