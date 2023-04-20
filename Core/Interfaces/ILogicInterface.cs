using SourceSharp.Core.Models;
using System.Collections.Generic;

namespace SourceSharp.Core.Interfaces;

internal interface ILogicInterface
{
    void Initialize(List<SourceSharpPlugin> plugins);
    void Shutdown();
}
