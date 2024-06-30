using Core.ViewModels.Base;
using Models.Json;
using MvvmCross.Navigation;
using Serilog;

namespace Core.ViewModels;

public class DashboardViewModel : BaseViewModel {
    private readonly ILogger _logger;

    public DashboardViewModel(IMvxNavigationService navigationService, AppConfigModel config, ILogger logger) : base(navigationService, config) {
        _logger = logger;
    }
}
