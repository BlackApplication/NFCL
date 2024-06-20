using Core.Configuration;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Core.ViewModels;

public class BaseViewModel : MvxViewModel {
    protected readonly IMvxNavigationService _navigationService;

    public string CurentAppVersion { get; }

    public BaseViewModel(IMvxNavigationService navigationService, AppConfigModel config) {
        _navigationService = navigationService;

        CurentAppVersion = config.Version;
    }
}
