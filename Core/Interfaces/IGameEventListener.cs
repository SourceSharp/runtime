using SourceSharp.Sdk.Models;

namespace SourceSharp.Core.Interfaces;

internal interface IGameEventListener : IListenerBase
{
    /// <summary>
    /// 事件触发时调用
    /// </summary>
    /// <param name="event">CGameEvent</param>
    /// <returns>True = 阻止事件</returns>
    bool OnFireEvent(GameEvent @event);
}
