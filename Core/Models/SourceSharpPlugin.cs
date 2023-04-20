using McMaster.NETCore.Plugins;
using SourceSharp.Sdk.Enums;
using SourceSharp.Sdk.Interfaces;

namespace SourceSharp.Core.Models;

internal class SourceSharpPlugin
{
    public required string Path { get; set; }

    public required PluginLoader Loader { get; set; }

    public required IPlugin Instance { get; set; }

    public PluginStatus Status { get; set; } = PluginStatus.None;


    // copyright
    public required string Name { get; set; }
    public required string Author { get; set; }
    public required string Version { get; set; }
    public string Url { get; set; } = "https://github.com/SourceSharp";
    public string Description { get; set; } = string.Empty;
}
