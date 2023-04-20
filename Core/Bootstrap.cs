using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SourceSharp.Core.Configurations;
using SourceSharp.Core.Interfaces;
using SourceSharp.Core.Modules;
using SourceSharp.Sdk.Interfaces;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace SourceSharp.Core;

internal static class Bootstrap
{
    private static readonly CancellationTokenSource CancellationTokenSource = new();

    [UnmanagedCallersOnly]
    public static void InitializeSourceSharp()
        => Initialize();

    [UnmanagedCallersOnly]
    public static void ShutdownSourceSharp() => CancellationTokenSource.Cancel(false);

    private static void Initialize()
    {
        // bin
        var dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!;
        // root
        var parent = Directory.GetParent(dir)!.Name;
        // config
        var path = Path.Combine(parent, "configs", "core.json");

        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().AddJsonFile(path).Build();

        ConfigureServices(services, configuration);

        var serviceProvider = services.BuildServiceProvider();

        Boot(serviceProvider);
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration.GetSection("Core").Get<CoreConfig>() ??
                              throw new InvalidDataException("Core config is missing!"));

        services.AddSingleton<ICore, CoreLogic>();

        services.AddSingleton<IGameEventListener, GameEventListener>();
        services.AddSingleton<IPluginManager, PluginManager>();
    }

    private static void Boot(IServiceProvider services)
    {
        var pluginManager = services.GetRequiredService<IPluginManager>();

        pluginManager.Initialize();

        Task.Run(async () => await SignalThread(services));
    }

    private static async Task SignalThread(IServiceProvider services)
    {
        var pluginManager = services.GetRequiredService<IPluginManager>();

        while (true)
        {
            await Task.Delay(TimeSpan.FromMicroseconds(1));

            if (!CancellationTokenSource.IsCancellationRequested)
            {
                pluginManager.Signal();
                continue;
            }

            pluginManager.Shutdown();
            break;
        }
    }
}