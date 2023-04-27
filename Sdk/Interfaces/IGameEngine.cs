namespace SourceSharp.Sdk.Interfaces;

// TODO  FindMap

public interface IGameEngine : IRuntime
{
    /// <summary>
    /// 当前地图的运行时间
    /// </summary>
    /// <param name="readFromPtr">使用指针读取</param>
    /// <returns>gpGlobals->curtime 或 缓存的值</returns>
    float GetGameTime(bool readFromPtr = false);

    /// <summary>
    /// 获取引擎时间
    /// </summary>
    /// <returns>Plat_Time</returns>
    float GetEngineTime();

    /// <summary>
    /// 上一帧花费的时间
    /// </summary>
    /// <returns>engine time span</returns>
    float GetGameFrameTime();

    /// <summary>
    /// 当前地图运行的帧数
    /// </summary>
    /// <param name="readFromPtr">使用指针读取</param>
    /// <returns>gpGlobals->tickCount 或 缓存的值</returns>
    int GetGameTickCount(bool readFromPtr = false);

    /// <summary>
    /// 检查地图是否是有效的bsp地图
    /// </summary>
    /// <param name="map">文件名 (不含.bsp)</param>
    /// <returns>True = Valid</returns>
    bool IsMapValid(string map);

    /// <summary>
    /// 获取当前的地图
    /// </summary>
    /// <returns>地图名,  不含.bsp</returns>
    string GetCurrentMap();

    /// <summary>
    /// 预加载模型
    /// </summary>
    /// <param name="model">路径</param>
    /// <param name="preLoad">预加载</param>
    /// <returns>Index in StringTable</returns>
    int PrecacheModel(string model, bool preLoad = false);

    /// <summary>
    /// 预加载Decal
    /// </summary>
    /// <param name="decal">路径</param>
    /// <param name="preLoad">预加载</param>
    /// <returns>Index in StringTable</returns>
    int PrecacheDecal(string decal, bool preLoad = false);

    /// <summary>
    /// 预加载Generic
    /// </summary>
    /// <param name="generic">路径</param>
    /// <param name="preLoad">预加载</param>
    /// <returns>Index in StringTable</returns>
    int PrecacheGeneric(string generic, bool preLoad = false);

    /// <summary>
    /// 预加载音频
    /// </summary>
    /// <param name="sound">路径</param>
    /// <param name="preLoad">预加载</param>
    /// <returns>Index in StringTable</returns>
    bool PrecacheSound(string sound, bool preLoad = false);

    /// <summary>
    /// 模型是否已被预加载
    /// </summary>
    /// <param name="model">路径</param>
    bool IsModelPreCached(string model);

    /// <summary>
    /// Decal是否已被已加载
    /// </summary>
    /// <param name="decal">路径</param>
    bool IsDecalPreCached(string decal);

    /// <summary>
    /// Generic是否已被预加载
    /// </summary>
    /// <param name="generic">路径</param>
    bool IsGenericPreCached(string generic);

    /// <summary>
    /// 更换地图
    /// </summary>
    /// <param name="map">地图名</param>
    /// <param name="reason">更换的原因</param>
    bool ChangeLevel(string map, string reason);
}
