using Core.Configuration;
using Models.Api;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Services.Interfaces;
using System.Diagnostics;

namespace Core.ViewModels;

public class LoginViewModel : BaseViewModel {
    #region Variables

    private string _email = "";
    public string Email {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    private string _password = "";
    public string Password {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    private bool _isErrorVisible;
    public bool IsErrorVisible {
        get => _isErrorVisible;
        set => SetProperty(ref _isErrorVisible, value);
    }

    public MvxCommand GoToSiteCommand { get; }

    public MvxAsyncCommand LoginCommand { get; }

    private readonly IAuthService _authService;

    #endregion

    public LoginViewModel(IMvxNavigationService navigationService, AppConfigModel config, IAuthService authService) : base(navigationService, config) {
        _authService = authService;

        GoToSiteCommand = new MvxCommand(GoToSite);
        LoginCommand = new MvxAsyncCommand(Login);
    }

    private async Task Login() {
        var loginData = new LoginModel { Email = Email, Password = Password };

        var user = await _authService.LoginAsync(loginData);
    }

    private void GoToSite() {
        OpenBrowser("https://nightfallcraft.com/");
    }

    private static void OpenBrowser(string url) {
        try {
            Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
        } catch (Exception) { }
    }

    private void OnToggleVisibility() {
        IsErrorVisible = !IsErrorVisible;
    }
}
