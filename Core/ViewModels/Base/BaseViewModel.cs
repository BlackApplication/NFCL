using Models.Json;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Serilog;

namespace Core.ViewModels.Base;

public abstract class BaseViewModel : MvxViewModel
{
    protected readonly IMvxNavigationService _navigationService;
    protected readonly ILogger _logger;

    public string CurentAppVersion { get; }

    public BaseViewModel(IMvxNavigationService navigationService, AppConfigModel config, ILogger logger) {
        _navigationService = navigationService;
        _logger = logger;

        CurentAppVersion = config.Version;
    }

    public void LogInfo(string text, params object?[] values) {
        _logger.Information(text, values);
    }
}

public abstract class BaseViewModel<T> : MvxViewModel<T>
{
    protected readonly IMvxNavigationService _navigationService;
    protected readonly ILogger _logger;

    public string CurentAppVersion { get; }

    public BaseViewModel(IMvxNavigationService navigationService, AppConfigModel config, ILogger logger) {
        _navigationService = navigationService;
        _logger = logger;

        CurentAppVersion = config.Version;
    }

    public void LogInfo(string text, params object?[] values) {
        _logger.Information(text, values);
    }
}
