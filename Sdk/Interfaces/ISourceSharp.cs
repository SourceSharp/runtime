using SourceSharp.Sdk.Enums;
using SourceSharp.Sdk.Models;
using System;

namespace SourceSharp.Sdk.Interfaces;

public interface ISourceSharp : IRuntime
{
    /// <summary>
    /// 获取游戏Mod位置
    /// e.g. csgo/insurgency
    /// </summary>
    /// <returns>Abs路径</returns>
    string GetGamePath();

    /// <summary>
    /// 获取SRCDS根目录路径
    /// </summary>
    /// <returns>Abs路径</returns>
    string GetRootPath();

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

    /// <summary>
    /// 获取最大玩家人数
    /// </summary>
    /// <returns>最大玩家人数</returns>
    int GetMaxClients();

    /// <summary>
    /// 获取最大真实人类玩家数
    /// </summary>
    /// <returns></returns>
    int GetMaxHumanPlayers();

    /// <summary>
    /// 获取当前的游戏
    /// </summary>
    /// <returns>EngineVersion</returns>
    GameEngineVersion GetEngineVersion();

    /// <summary>
    /// 执行服务端命令
    /// </summary>
    void ExecuteServerCommand(string command);

    /// <summary>
    /// 添加命令到服务器命令缓冲池
    /// </summary>
    /// <param name="command"></param>
    void InsertServerCommand(string command);

    /// <summary>
    /// 执行并清理服务器命令缓冲池
    /// </summary>
    void ServerExecute();

    /// <summary>
    /// 查找ConVar
    /// </summary>
    /// <param name="name">ConVar名</param>
    /// <returns>ConVar实例</returns>
    ConVar? FindConVar(string name);

    /// <summary>
    /// 创建游戏事件.
    /// 该事件如果没有调用Fire,
    /// 则必须要使用Cancel进行关闭.
    /// </summary>
    /// <param name="name">事件名</param>
    /// <param name="broadcast">设置默认发送值</param>
    /// <returns>GameEvent实例</returns>
    GameEvent? CreateEvent(string name, bool broadcast);
}
