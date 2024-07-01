using Core.ViewModels.Base;
using Models.Json;
using MvvmCross.Navigation;
using Serilog;

namespace Core.ViewModels;

public class MainViewModel : BaseViewModel {
    public MainViewModel(IMvxNavigationService navigationService, AppConfigModel config, ILogger logger) : base(navigationService, config, logger) {

    }
}
