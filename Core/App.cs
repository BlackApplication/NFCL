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

namespace Core;

public class App : MvxApplication {
    public override void Initialize() {
        var config = FileHelper.ReadJsonFile<AppConfigModel>("Configuration/config.json");
        Mvx.IoCProvider?.RegisterSingleton(config);
        var currentUserState = new CurrentUserState();
        Mvx.IoCProvider?.RegisterSingleton(currentUserState);
        var ServersState = new ServersState();
        Mvx.IoCProvider?.RegisterSingleton(ServersState);
        var logger = Log.Logger;
        Mvx.IoCProvider?.RegisterSingleton(logger);
        Mvx.IoCProvider?.RegisterSingleton<IHttpService>(new HttpService(config));

        Mvx.IoCProvider?.RegisterType<IAuthService, AuthService>();
        Mvx.IoCProvider?.RegisterType<ILauncherApi, LauncherApi>();

        RegisterAppStart<WelcomeViewModel>();
    }
}
