using System;

namespace SourceSharp.Sdk.Interfaces;

public interface ICore
{
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
    /// 在主线程调用Action
    /// </summary>
    /// <param name="action">action</param>
    void Invoke(Action action);
}

