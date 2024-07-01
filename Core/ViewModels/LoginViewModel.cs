using Core.ViewModels.Base;
using Models.Api;
using Models.Json;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Serilog;
using Services.Api.Interfaces;
using Services.Helper;
using Services.States;

namespace Core.ViewModels;

public class LoginViewModel : BaseViewModel<Action> {

    #region Services

    private readonly IAuthService _authService;

    private readonly AppConfigModel _config;

    private readonly CurrentUserState _userState;

    #endregion

    #region Variables

    private Action _afterLogin = null!;

    private string _email = "";
    public string Email {
        get => _email;
        set => SetProperty(ref _email, value, () => IsErrorVisible = false);
    }

    private string _password = "";
    public string Password {
        get => _password;
        set => SetProperty(ref _password, value, () => IsErrorVisible = false);
    }

    private bool _isErrorVisible;
    public bool IsErrorVisible {
        get => _isErrorVisible;
        set => SetProperty(ref _isErrorVisible, value, () => RaisePropertyChanged(() => ErrorVisible));
    }

    public string ErrorVisible {
        get {
            if (IsErrorVisible) {
                return "visible";
            }

            return "collapsed";
        }
    }

    #endregion

    #region Commands

    public MvxCommand GoToSiteCommand { get; }

    public MvxAsyncCommand LoginCommand { get; }

    #endregion

    #region Constructor

    public LoginViewModel(IMvxNavigationService navigationService, AppConfigModel config, IAuthService authService, CurrentUserState state, ILogger logger) : base(navigationService, config, logger) {
        _authService = authService;
        _config = config;
        _userState = state;

        GoToSiteCommand = new MvxCommand(GoToSite);
        LoginCommand = new MvxAsyncCommand(Login);
    }

    #endregion

    public override void Prepare(Action afterLogin) {
        _afterLogin = afterLogin;
    }

    private async Task Login() {
        var loginData = new LoginModel { Email = Email, Password = Password, IsLauncher = true };

        var user = await _authService.LoginAsync(loginData);
        if (user is null) {
            IsErrorVisible = true;
            return;
        }

        _userState.CurrentUser = user;

        _afterLogin.Invoke();
        await _navigationService.Close(this);
    }


    private void GoToSite() {
        BrowserHelper.OpenBrowser(_config.ServerUrl);
    }
}
