using Core.Configuration;
using Core.States.Interfaces;
using Models.Api;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Services.Interfaces;
using Services.States;
using System.Diagnostics;

namespace Core.ViewModels;

public class LoginViewModel : BaseViewModel {
    #region Variables

    private string _email = "";
    public string Email {
        get => _email;
        set => SetProperty(ref _email, value, () => {
            IsErrorVisible = false;
        });
    }

    private string _password = "";
    public string Password {
        get => _password;
        set => SetProperty(ref _password, value, () => {
            IsErrorVisible = false;
        });
    }

    private bool _isErrorVisible;
    public bool IsErrorVisible {
        get => _isErrorVisible;
        set => SetProperty(ref _isErrorVisible, value);
    }

    public MvxCommand GoToSiteCommand { get; }

    public MvxAsyncCommand LoginCommand { get; }

    private readonly IAuthService _authService;

    private readonly AppConfigModel _config;

    private readonly CurrentUserState _currentUserState;

    #endregion

    public LoginViewModel(IMvxNavigationService navigationService, AppConfigModel config, IAuthService authService, CurrentUserState state) : base(navigationService, config) {
        _authService = authService;
        _config = config;
        _currentUserState = state;

        GoToSiteCommand = new MvxCommand(GoToSite);
        LoginCommand = new MvxAsyncCommand(Login);
    }

    private async Task Login() {
        var loginData = new LoginModel { Email = Email, Password = Password };

        var user = await _authService.LoginAsync(loginData);
        if (user is null) {
            IsErrorVisible = true;
            return;
        }

        _currentUserState.CurrentUser = user;

        await _navigationService.Navigate<ServersViewModel>();
    }

    private void GoToSite() {
        OpenBrowser(_config.ServerUrl);
    }

    private static void OpenBrowser(string url) {
        try {
            Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
        } catch (Exception) { }
    }
}
