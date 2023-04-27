using System;
using System.Threading.Tasks;

namespace SourceSharp.Core.Test;

internal static class Program
{
    static async Task Main()
    {
        Bootstrap.InitializeTest();
        await Task.Delay(TimeSpan.FromDays(1));
    }
}
