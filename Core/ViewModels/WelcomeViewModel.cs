using Core.States;
using Core.ViewModels.Base;
using Models.Json;
using MvvmCross.Navigation;
using Services;
using Services.Api.Interfaces;
using Services.Helper;
using Services.States;
using System.ComponentModel;

namespace Core.ViewModels;

public class WelcomeViewModel : BaseViewModel, INotifyPropertyChanged {

    #region Services

    private readonly IAuthService _authService;
    private readonly LauncherService _laucnherService;
    private readonly ServersState _serversState;
    private readonly CurrentUserState _userState;

    #endregion

    #region Variables

    public string LoadTextColor {
        get {
            if (LoadText == "Вход успешный!") {
                return "LightGreen";
            }

            return "White";
        }
    }

    private string _loadText = null!;
    public string LoadText {
        get => _loadText;
        set => SetProperty(ref _loadText, value, () => RaisePropertyChanged(() => LoadTextColor));
    }

    #endregion

    #region Constructor

    public WelcomeViewModel(IMvxNavigationService navigationService, IAuthService authService, ILauncherApi launcherApi, AppConfigModel config, ServersState serversState, CurrentUserState userState) : base(navigationService, config) {
        _laucnherService = new LauncherService(Directory.GetCurrentDirectory(), launcherApi);
        _serversState = serversState;
        _authService = authService;
        _userState = userState;
    }

    #endregion

    public override void ViewAppeared() {
        Task.Run(() => InitialLoad());
    }

    private async Task InitialLoad() {
        UpdateLoadText("Получение версии...");
        var isActual = await CheckIfLaucherVersionActualAsync();
        if (!isActual) {
            UpdateLauncher();
        }

        UpdateLoadText("Проверка файлов...");
        CheckGameDirectory();

        UpdateLoadText("Получение списка серверов...");
        await GetServersListAsync();

        UpdateLoadText("Попытка входа...");
        await TryToGetRememberUser();

        if (_userState.CurrentUser != null) {
            AfterLoginAction();
        } else {
            await OpenLoginWindowAsync(AfterLoginAction);
        }
    }

    private async Task TryToGetRememberUser() {
        var user = await _authService.GetCurrentUserAsync();

        _userState.CurrentUser = user;
    }

    private void AfterLoginAction() {
        Task.Run(() => {
            UpdateLoadText("Вход успешный!");
            _navigationService.Navigate<DashboardViewModel>();
        });
    }

    private async Task OpenLoginWindowAsync(Action action) {
        await _navigationService.Navigate<LoginViewModel, Action>(action);
    }

    private void UpdateLoadText(string text) {
        LoadText = text;
        Thread.Sleep(500);
    }

    private async Task GetServersListAsync() {
        var servers = await _laucnherService.GetServersListAsync();

        _serversState.Servers = servers;
    }

    private async Task<bool> CheckIfLaucherVersionActualAsync() {
        var serverVersion = await _laucnherService.GetActualVersionAsync();

        return serverVersion == CurentAppVersion;
    }

    private static void CheckGameDirectory() {
        var gamePath = PathHelper.GetGamePath();
        var cachePath = PathHelper.GetCachePath();

        if (!Directory.Exists(gamePath)) {
            Directory.CreateDirectory(gamePath);
        }

        if (!File.Exists(cachePath)) {
            using (File.Create(cachePath)) { }
        }
    }

    private void UpdateLauncher() {
       // TODO update
    }
}
