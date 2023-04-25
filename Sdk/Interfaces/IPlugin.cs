namespace SourceSharp.Sdk.Interfaces;

public interface IPlugin
{
    /// <summary>
    /// 初始化加载...
    /// </summary>
    /// <returns>True = 成功</returns>
    bool OnLoad();

    /// <summary>
    /// 在所有Plugin加载完成后调用
    /// </summary>
    void OnAllLoad();

    /// <summary>
    /// 查询当前插件是否可用
    /// </summary>
    /// <returns>False = 插件失败</returns>
    bool QueryRunning();

    /// <summary>
    /// 卸载时调用
    /// </summary>
    void OnShutdown();

    /// <summary>
    /// Interface被卸载时调用
    /// </summary>
    /// <param name="interface">interface 实例</param>
    void NotifyInterfaceDrop(IRuntime @interface);
}

public abstract class PluginBase : IPlugin
{
#nullable disable
    protected readonly ISourceSharp _sourceSharp;
    protected readonly IShareSystem _shareSystem;
#nullable restore

    public virtual bool OnLoad()
    {
        return true;
    }

    public virtual void OnAllLoad()
    {

    }

    public virtual void OnShutdown()
    {

    }

    public virtual bool QueryRunning()
    {
        return true;
    }

    public virtual void NotifyInterfaceDrop(IRuntime @interface)
    {

    }
}