using Core.Configuration;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
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

    #endregion

    public LoginViewModel(IMvxNavigationService navigationService, AppConfig config) : base(navigationService, config) {
        GoToSiteCommand = new MvxCommand(GoToSite);
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
