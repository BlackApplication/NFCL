using Core.Configuration;
using Core.ViewModels;
using MvvmCross;
using MvvmCross.ViewModels;
using Services.Implementations;
using Services.Interfaces;

namespace Core;

public class App : MvxApplication {
    public override void Initialize() {
        var config = ConfigLoader.Load("Configuration/config.json");
        Mvx.IoCProvider?.RegisterSingleton(config);
        Mvx.IoCProvider?.RegisterType<IHttpService, HttpService>();
        Mvx.IoCProvider?.RegisterType<IAuthService, AuthService>();

        RegisterAppStart<WelcomeViewModel>();
    }
}
