using Models.Json;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Core.ViewModels.Base;

public abstract class BaseViewModel : MvxViewModel
{
    protected readonly IMvxNavigationService _navigationService;

    public string CurentAppVersion { get; }

    public BaseViewModel(IMvxNavigationService navigationService, AppConfigModel config)
    {
        _navigationService = navigationService;

        CurentAppVersion = config.Version;
    }
}

public abstract class BaseViewModel<T> : MvxViewModel<T>
{
    protected readonly IMvxNavigationService _navigationService;

    public string CurentAppVersion { get; }

    public BaseViewModel(IMvxNavigationService navigationService, AppConfigModel config)
    {
        _navigationService = navigationService;

        CurentAppVersion = config.Version;
    }
}
