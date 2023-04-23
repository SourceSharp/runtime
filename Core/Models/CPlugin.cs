using McMaster.NETCore.Plugins;
using SourceSharp.Sdk.Attributes;
using SourceSharp.Sdk.Enums;
using SourceSharp.Sdk.Interfaces;
using System;
using System.Text.Json;

namespace SourceSharp.Core.Models;

internal class CPlugin
{
    public string Path { get; }

    public PluginLoader Loader { get; }

    public IPlugin Instance { get; }

    public PluginStatus Status { get; set; } = PluginStatus.None;

    public Action<bool>? FrameHook { get; }

    // copyright
    public string Name { get; set; }
    public string Author { get; set; }
    public string Version { get; set; }
    public string Url { get; set; } = "https://github.com/SourceSharp";
    public string Description { get; set; } = string.Empty;

    public CPlugin(string path, PluginLoader loader, IPlugin instance,
        Action<bool>? frameHook,
        PluginAttribute pa)
    {
        Path = path;
        Loader = loader;
        Instance = instance;
        Status = PluginStatus.Checked;
        FrameHook = frameHook;

        Name = pa.Name;
        Author = pa.Author;
        Version = pa.Version;
        Url = pa.Url;
        Description = pa.Description;
    }

    public override string ToString()
        => JsonSerializer.Serialize(new { Name, Author, Version, Url, Description }, new JsonSerializerOptions { WriteIndented = true });

    public override int GetHashCode()
        => Path.GetHashCode();

    public override bool Equals(object? obj)
    {
        if (obj is CPlugin p)
        {
            return p.GetHashCode() == GetHashCode();
        }

        return false;
    }

    public static bool operator ==(CPlugin obj1, CPlugin obj2)
            => obj1.Equals(obj2);
    public static bool operator !=(CPlugin obj1, CPlugin obj2)
        => !obj1.Equals(obj2);
}
