using System.Text.Json.Serialization;

namespace SourceSharp.Core.Configurations;
internal sealed class CoreConfig
{
    [JsonRequired]
    public bool AllowUnsafe { get; set; }
}
