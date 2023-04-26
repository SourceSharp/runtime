using SourceSharp.Core.Models;

namespace SourceSharp.Core.Interfaces;

internal interface IGameEventListener : IListenerBase
{
    /// <summary>
    /// 事件触发时调用
    /// </summary>
    /// <param name="event">CGameEvent</param>
    /// <returns>True = 阻止事件</returns>
    bool OnEventFire(CGameEvent @event);

    /// <summary>
    /// 事件触发后调用
    /// </summary>
    /// <param name="event">CGameEvent</param>
    void OnEventFired(CGameEvent @event);
}
