using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SourceSharp.Core.Configurations;
using SourceSharp.Core.Interfaces;
using SourceSharp.Core.Modules;
using SourceSharp.Core.Utils;
using SourceSharp.Sdk.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SourceSharp.Core;

public static class Bootstrap
{
    private static bool _isShutdown;
    private static Task? _signalTask;

    [UnmanagedCallersOnly]
    public static int InitializeSourceSharp() => Initialize();

    [UnmanagedCallersOnly]
    public static void ShutdownSourceSharp()
    {
        _isShutdown = true;
        _signalTask?.Wait();
    }

    public static int InitializeTest() => Initialize();

    private static int Initialize()
    {
        _isShutdown = false;

        try
        {
            var root = Path.Combine(Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().Location)!,
                    Bridges.SourceSharp.GetGamePath(),
                    "addons",
                    "sourcesharp");

            // config
            var path = Path.Combine(root, "configs", "core.json");

            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().AddJsonFile(path).Build();

            ConfigureServices(services, configuration);

            var serviceProvider = services.BuildServiceProvider(options: new()
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });

            Debug.Print("MaxClients is " + Bridges.SourceSharp.GetMaxClients());
            Debug.Print("MaxHumanPlayers is " + Bridges.SourceSharp.GetMaxHumanPlayers());

            Boot(serviceProvider);

            return 0;
        }
        catch (Exception ex)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss} [SourceSharp]  Failed to init SourceSharp.");
            Console.ForegroundColor = color;
            Console.WriteLine(ex.ToString());
            Console.WriteLine(Environment.NewLine);
            return 1;
        }
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration.GetSection("Core").Get<CoreConfig>() ??
                              throw new InvalidDataException("Core config is missing!"));

        services.AddSingleton<ISourceSharpBase, SourceSharp>();
        services.AddSingleton<IShareSystemBase, ShareSystem>();

        services.AddSingleton<IAdminManager, AdminManager>();
        services.AddSingleton<ICommandListener, CommandListener>();
        //services.AddSingleton<IConVarManager, ConVarManager>();
        //services.AddSingleton<IGameEventListener, GameEventListener>();
        services.AddSingleton<IPlayerListener, PlayerListener>();
        services.AddSingleton<IPlayerManagerBase, PlayerManager>();

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
        services.GetRequiredService<IPluginManager>().Initialize(services);

        // export caller invoker
        Invoker.Initialize(services);

        _signalTask = Task.Run(async () => await SignalThread(services));
    }

    private static async Task SignalThread(IServiceProvider services)
    {
        var pluginManager = services.GetRequiredService<IPluginManager>();

        while (true)
        {
            await Task.Delay(TimeSpan.FromMicroseconds(1));

            if (_isShutdown)
            {
                // Shutdown !!!
                foreach (var module in services.GetAllServices<IModuleBase>())
                {
                    module.Shutdown();
                }
                services.GetRequiredService<IPluginManager>().Shutdown();
                return;
            }

            pluginManager.Signal();
        }
    }
}