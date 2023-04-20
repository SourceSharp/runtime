namespace SourceSharp.Sdk.Interfaces;

public interface IShareSystem
{
    /// <summary>
    /// 注册Interface到依赖注入Scope
    /// </summary>
    /// <param name="interface">IRuntime实现</param>
    /// <param name="@plugin">Plugin实例</param>
    void AddInterface(IRuntime @interface, IPlugin @plugin);

    /// <summary>
    /// 获取必须的Interface, 不为空
    /// </summary>
    /// <typeparam name="T">IRuntime实现的Interface</typeparam>
    /// <returns>IRuntime实现</returns>
    T GetRequiredInterface<T>(uint version) where T : IRuntime;

    /// <summary>
    /// 获取可选的Interface, 可空
    /// </summary>
    /// <typeparam name="T">IRuntime实现的Interface</typeparam>
    /// <returns>IRuntime实现</returns>
    T? GetInterface<T>(uint version) where T : IRuntime;
}
