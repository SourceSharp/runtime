using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SourceSharp.Core.Configurations;
using SourceSharp.Core.Interfaces;
using SourceSharp.Core.Modules;
using SourceSharp.Core.Utils;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace SourceSharp.Core;

public static class Bootstrap
{
    private static readonly CancellationTokenSource CancellationTokenSource = new();

    [UnmanagedCallersOnly]
    public static void InitializeSourceSharp()
        => Initialize();

    [UnmanagedCallersOnly]
    public static void ShutdownSourceSharp() => CancellationTokenSource.Cancel(false);

    public static void InitializeTest() => Initialize();

    private static void Initialize()
    {
        // bin
        var dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!;
        // root
        var parent = Directory.GetParent(dir)!.Name;
        // config
        var path = Path.Combine(parent, "configs", "core.json");

#if DEBUG
        if (!File.Exists(path))
        {
            path = Path.Combine(dir, "config.json");
        }
#endif

        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().AddJsonFile(path).Build();

        ConfigureServices(services, configuration);

        var serviceProvider = services.BuildServiceProvider(options: new ServiceProviderOptions
        {
            ValidateOnBuild = true,
            ValidateScopes = true
        });

        Boot(serviceProvider);
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration.GetSection("Core").Get<CoreConfig>() ??
                              throw new InvalidDataException("Core config is missing!"));

        services.AddSingleton<ISourceSharpBase, SourceSharp>();
        services.AddSingleton<IShareSystemBase, ShareSystem>();

        services.AddSingleton<IGameEventListener, GameEventListener>();
        services.AddSingleton<ICommandListener, CommandListener>();
        services.AddSingleton<IPlayerListener, PlayerListener>();
        services.AddSingleton<IPlayerManagerBase, PlayerManager>();
        services.AddSingleton<IAdminManagerBase, AdminManager>();

        services.AddSingleton<IPluginManager, PluginManager>();
    }

    private static void Boot(IServiceProvider services)
    {

        // Init IModuleBase
        foreach (var module in services.GetAllServices<IModuleBase>())
        {
            module.Initialize();
        }

        // Plugin Manager should be the LAST!
        services.GetRequiredService<IPluginManager>().Initialize();
        Invoker.Initialize(services);

        Task.Run(async () => await SignalThread(services));

#if DEBUG
        Console.WriteLine("Boot!");
#endif
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

            // Shutdown !!!
            foreach (var module in services.GetAllServices<IModuleBase>())
            {
                module.Shutdown();
            }
            services.GetRequiredService<IPluginManager>().Shutdown();
            break;
        }
    }
}