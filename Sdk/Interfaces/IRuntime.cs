namespace SourceSharp.Sdk.Interfaces;

public interface IRuntime
{
    /// <summary>
    /// 获取Interface版本
    /// </summary>
    /// <returns>版本</returns>
    uint GetInterfaceVersion();

    /// <summary>
    /// 获取Interface名称
    /// </summary>
    /// <returns>名称</returns>
    string GetInterfaceName();
}
