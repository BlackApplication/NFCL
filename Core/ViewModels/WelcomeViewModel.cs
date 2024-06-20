using Core.Configuration;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace Core.ViewModels;

public class WelcomeViewModel : BaseViewModel {
    public IMvxAsyncCommand ShowLogin { get; }

    private async Task GoToLogin() {
        await _navigationService.Navigate<LoginViewModel>();
    }

    public WelcomeViewModel(IMvxNavigationService navigationService, AppConfig config) : base(navigationService, config) {
        ShowLogin = new MvxAsyncCommand(GoToLogin);
    }
}
