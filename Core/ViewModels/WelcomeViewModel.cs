using Core.ViewModels.Base;
using Models;
using Models.Json;
using MvvmCross.Navigation;
using Serilog;
using Services;
using Services.Api.Interfaces;
using Services.Helper;
using Services.States;
using Services.States.Interfaces;

namespace Core.ViewModels;

public class WelcomeViewModel : BaseViewModel {

    #region Services

    private readonly IAuthService _authService;
    private readonly IDownloadState _downloadState;
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

    private bool _isUpdating;
    public bool IsUpdating {
        get => _isUpdating;
        set => SetProperty(ref _isUpdating, value, () => RaisePropertyChanged(() => UpdateBarVisible));
    }

    public string UpdateBarVisible {
        get {
            if (IsUpdating) {
                return "visible";
            }

            return "collapsed";
        }
    }

    private int _loadPrecent;
    public int LoadPrecent {
        get => _loadPrecent;
        set => SetProperty(ref _loadPrecent, value);
    }

    #endregion

    #region Constructor

    public WelcomeViewModel(IMvxNavigationService navigationService, IAuthService authService, ILauncherApi launcherApi, AppConfigModel config, ServersState serversState, CurrentUserState userState, ILogger logger, LauncherService launcherService, DownloadState downloadState) : base(navigationService, config, logger) {
        _laucnherService = launcherService;
        _serversState = serversState;
        _authService = authService;
        _userState = userState;
        _downloadState = downloadState;
    }

    #endregion

    public override void ViewAppeared() {
        LogInfo("[Welcome] Начало инициализации...");
        Task.Run(() => InitialLoad());
    }

    private async Task InitialLoad() {
        UpdateLoadText("Проверка файлов...");
        CheckGameDirectory();

        UpdateLoadText("Получение версии...");
        var isActual = await CheckIfLaucherVersionActualAsync();
        if (!isActual) {
            UpdateLauncher();
            return;
        }

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
        LogInfo("[Welcome] Получение запомненого пользователя {0}", user == null ? "неудачно" : "успешно");

        _userState.CurrentUser = user;
    }

    private void AfterLoginAction() {
        Task.Run(() => {
            LogInfo("[Welcome] Вход как {0}", _userState.CurrentUser?.Name);
            UpdateLoadText("Вход успешный!");
            _navigationService.Navigate<DashboardViewModel>();
        });
    }

    private async Task OpenLoginWindowAsync(Action action) {
        LogInfo("[Welcome] Вход");
        await _navigationService.Navigate<LoginViewModel, Action>(action);
    }

    private void UpdateLoadText(string text) {
        LoadText = text;
        LogInfo("[Welcome] Инициализация: {0}", text);
        Thread.Sleep(500);
    }

    private async Task GetServersListAsync() {
        var servers = await _laucnherService.GetServersListAsync();
        LogInfo("[Welcome] Получена информация о серверах: {0}", servers.Count);

        _serversState.Servers = servers;
    }

    private async Task<bool> CheckIfLaucherVersionActualAsync() {
        var serverVersion = await _laucnherService.GetActualVersionAsync();
        var isActual = serverVersion == CurentAppVersion;
        LogInfo("[Welcome] Текущая версия {0} {1}", CurentAppVersion, isActual ? "актуальна" : $"устарела. Актуальная версия {serverVersion}");

        return isActual;
    }

    private static void CheckGameDirectory() {
        var gamePath = PathHelper.GetGamePath();
        var modsPath = Path.Combine(gamePath, "mods");

        if (!Directory.Exists(gamePath)) {
            Directory.CreateDirectory(gamePath);
        }

        if (!Directory.Exists(modsPath)) {
            Directory.CreateDirectory(modsPath);
        }
    }

    private async void UpdateLauncher() {
        var updateHandler = UpdateProgressChanged;
        _downloadState.OnChagePrecent += updateHandler;

        IsUpdating = true;
        await _laucnherService.UpdateLauncherAsync();

        _downloadState.OnChagePrecent -= updateHandler;
    }

    private void UpdateProgressChanged() {
        LoadPrecent = _downloadState.Precent();
    }
}
