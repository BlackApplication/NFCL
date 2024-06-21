using Core.Configuration;
using Core.States.Interfaces;
using Core.ViewModels;
using MvvmCross;
using MvvmCross.ViewModels;
using Services.Implementations;
using Services.Interfaces;
using Services.States;

namespace Core;

public class App : MvxApplication {
    public override void Initialize() {
        var config = ConfigLoader.Load("Configuration/config.json");
        Mvx.IoCProvider?.RegisterSingleton(config);
        var currentUserState = new CurrentUserState();
        Mvx.IoCProvider?.RegisterSingleton(currentUserState);

        Mvx.IoCProvider?.RegisterType<IHttpService, HttpService>();
        Mvx.IoCProvider?.RegisterType<IAuthService, AuthService>();

        RegisterAppStart<WelcomeViewModel>();
    }
}
