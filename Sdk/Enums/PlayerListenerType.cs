namespace SourceSharp.Sdk.Enums;

public enum PlayerListenerType
{
    /// <summary>
    /// 是否允许建立链接
    /// </summary>
    ConnectHook,

    /// <summary>
    /// 建立连接时
    /// </summary>
    Connected,

    /// <summary>
    /// 加入游戏
    /// </summary>
    PutInServer,

    /// <summary>
    /// 验证Steam Ticket
    /// </summary>
    Authorized,

    /// <summary>
    /// 检查Steam且已经在游戏里
    /// </summary>
    PostAdminCheck,

    /// <summary>
    /// 尝试退出游戏时
    /// </summary>
    Disconnecting,

    /// <summary>
    /// 完全退出游戏
    /// </summary>
    Disconnected,

    /// <summary>
    /// 发送KV命令
    /// </summary>
    CommandKeyValues,

    /// <summary>
    /// 发送KV命令处理后
    /// </summary>
    CommandKeyValuesPost,

    /// <summary>
    /// 修改设置后
    /// </summary>
    SettingsChanged,
}
