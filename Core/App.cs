using Core.ViewModels;
using Models.Json;
using MvvmCross;
using MvvmCross.ViewModels;
using Serilog;
using Services;
using Services.Api.Implementations;
using Services.Api.Interfaces;
using Services.States;
using System.Text;

namespace Core;

public class App : MvxApplication {
    public override void Initialize() {
        var config = AddConfiguration();
        RegisterApiServices();
        RegisterSingletons(config);

        Console.OutputEncoding = Encoding.UTF8;

        RegisterAppStart<WelcomeViewModel>();
    }

    private static void RegisterApiServices() {
        Mvx.IoCProvider?.RegisterType<IAuthService, AuthService>();
        Mvx.IoCProvider?.RegisterType<ILauncherApi, LauncherApi>();
    }

    private static void RegisterSingletons(AppConfigModel config) {
        Mvx.IoCProvider?.RegisterSingleton(config);
        var currentUserState = new CurrentUserState();
        Mvx.IoCProvider?.RegisterSingleton(currentUserState);
        var serversState = new ServersState();
        Mvx.IoCProvider?.RegisterSingleton(serversState);
        var downloadState = new DownloadState();
        Mvx.IoCProvider?.RegisterSingleton(downloadState);

        var logger = Log.Logger;
        Mvx.IoCProvider?.RegisterSingleton(logger);
        Mvx.IoCProvider?.RegisterSingleton<IHttpService>(new HttpService(config, logger, downloadState));
        var client = new ClientService(logger);
        Mvx.IoCProvider?.RegisterSingleton(client);
        var launcherApi = Mvx.IoCProvider?.Resolve<ILauncherApi>() ?? throw new ArgumentNullException("Api not initialize");
        var launcherService = new LauncherService(launcherApi, logger, downloadState);
        Mvx.IoCProvider?.RegisterSingleton(launcherService);
    }
    
    private static AppConfigModel AddConfiguration() {
        return new AppConfigModel {
            Version = "0.0.5",
            GameDirectory = ".nightfallcraft",
            ServerUrl = "https://localhost:5050"
        };
    }
}
