using SourceSharp.Sdk.Enums;
using System;

namespace SourceSharp.Sdk.Interfaces;

public interface ISourceSharp : IRuntime
{
    /// <summary>
    /// 获取完整的游戏路径
    /// </summary>
    /// <returns>Abs路径</returns>
    string GetGamePath();

    /// <summary>
    /// 获取SourceSharp路径
    /// </summary>
    /// <returns>Abs路径</returns>
    string GetSourceSharpPath();

    /// <summary>
    /// Path.Combine封装, 可以快速格式化路径
    /// </summary>
    /// <param name="type">路径类型</param>
    /// <param name="format">参数</param>
    /// <returns>Abs路径</returns>
    string BuildPath(PathType type, params string[] format);

    /// <summary>
    /// 打印到控制台
    /// </summary>
    /// <param name="message">内容</param>
    void PrintLine(string message);

    /// <summary>
    /// 输出到日志文件 (Async)
    /// </summary>
    /// <param name="message">内容</param>
    void LogMessage(string message);

    /// <summary>
    /// 输出到错误日志 (Async)
    /// </summary>
    /// <param name="message">内容</param>
    void LogError(string message);

    /// <summary>
    /// 在主线程调用Action,
    /// 当前线程为主线程时立即调用,
    /// 否则将在下一帧开始时调用.
    /// </summary>
    /// <param name="action">action method</param>
    void Invoke(Action action);

    /// <summary>
    /// 在下一帧开始时由主线程调用
    /// </summary>
    /// <param name="action">action method</param>
    void InvokeNextFrame(Action action);


}
