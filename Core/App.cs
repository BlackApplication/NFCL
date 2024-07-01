using Core.States;
using Core.ViewModels;
using Models.Json;
using MvvmCross;
using MvvmCross.ViewModels;
using Serilog;
using Services.Api.Implementations;
using Services.Api.Interfaces;
using Services.Helper;
using Services.States;
using System.Text;

namespace Core;

public class App : MvxApplication {
    public override void Initialize() {
        RegisterSingletons();
        RegisterApiServices();

        Console.OutputEncoding = Encoding.UTF8;

        RegisterAppStart<WelcomeViewModel>();
    }

    private void RegisterSingletons() {
        var config = AddConfiguration();
        Mvx.IoCProvider?.RegisterSingleton(config);
        var currentUserState = new CurrentUserState();
        Mvx.IoCProvider?.RegisterSingleton(currentUserState);
        var ServersState = new ServersState();
        Mvx.IoCProvider?.RegisterSingleton(ServersState);
        var logger = Log.Logger;
        Mvx.IoCProvider?.RegisterSingleton(logger);
        Mvx.IoCProvider?.RegisterSingleton<IHttpService>(new HttpService(config));
    }

    private void RegisterApiServices() {
        Mvx.IoCProvider?.RegisterType<IAuthService, AuthService>();
        Mvx.IoCProvider?.RegisterType<ILauncherApi, LauncherApi>();
    }

    private AppConfigModel AddConfiguration() {
        return new AppConfigModel {
            Version = "0.0.2",
            GameDirectory = ".nightfallcraft",
            ServerUrl = "https://localhost:5050"
        };
    }
}
