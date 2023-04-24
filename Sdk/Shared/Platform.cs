
using System;
using System.Runtime.InteropServices;

namespace SourceSharp.Sdk.Shared;
public static class Platform
{
    // 留白
    // 难免要上lin, 这里先做一个最简单的实现
    // (c#应该有东西去实现这部分的功能吧?)
    public static string GetPluginSuffix()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "dll";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
        {
            return "so";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return "dylib";
        }
        throw new PlatformNotSupportedException();
    }

}
