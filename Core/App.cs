using Core.Configuration;
using Core.ViewModels;
using MvvmCross;
using MvvmCross.ViewModels;

namespace Core;

public class App : MvxApplication {
    public override void Initialize() {
        var config = ConfigLoader.Load("Configuration/config.json");
        Mvx.IoCProvider?.RegisterSingleton(config);

        RegisterAppStart<WelcomeViewModel>();
    }
}
