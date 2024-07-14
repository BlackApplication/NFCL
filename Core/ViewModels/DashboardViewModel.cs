using Core.States;
using Core.ViewModels.Base;
using Models;
using Models.Json;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Serilog;
using Services;
using Services.States;

namespace Core.ViewModels;

public class DashboardViewModel : BaseViewModel {

    #region Services

    private readonly ServersState _serversState;
    private readonly LauncherService _launcherService;
    private readonly ClientService _clientService;
    private readonly CurrentUserState _currentUserState;

    #endregion

    private bool _isServerList;
    private bool IsServerList {
        get => _isServerList;
        set => SetProperty(ref _isServerList, value, () => RaiseAllPropertiesChanged());
    }

    public string CurrentServerName => _serversState.CurrentServerName;

    public int CurrentServerUsers { get; set; }

    public int CurrentServerMaxUsers { get; set; }

    public string ServerChooseButtonVisible {
        get {
            if (CurrentServerName == string.Empty && !IsServerList) {
                return "visible";
            }

            return "collapsed";
        }
    }

    public string ServerChooseVisible {
        get {
            if (IsServerList) {
                return "visible";
            }

            return "collapsed";
        }
    }

    public string ServerVisible {
        get {
            if (CurrentServerName != string.Empty) {
                return "visible";
            }

            return "collapsed";
        }
    }

    public string PlayButtonVisible {
        get {
            if (!IsLaunched) {
                return "visible";
            }

            return "collapsed";
        }
    }

    public string ProgresBarVisible {
        get {
            if (IsLaunched) {
                return "visible";
            }

            return "collapsed";
        }
    }

    private bool _isLaunched;
    public bool IsLaunched {
        get => _isLaunched;
        set => SetProperty(ref _isLaunched, value, () => {
            RaisePropertyChanged(() => ProgresBarVisible);
            RaisePropertyChanged(() => PlayButtonVisible);
        });
    }

    private string _loadText;
    public string LoadText {
        get => _loadText;
        set => SetProperty(ref _loadText, value);
    }

    private int _loadPrecent;
    public int LoadPrecent {
        get => _loadPrecent;
        set => SetProperty(ref _loadPrecent, value);
    }

    #region Commands

    public MvxCommand GoToServerListCommand { get; }

    public MvxCommand ChooseProximaCommand { get; }

    public MvxAsyncCommand PlayCommand { get; }

    #endregion

    #region Constructor

    public DashboardViewModel(IMvxNavigationService navigationService, AppConfigModel config, ILogger logger, ServersState serversState, LauncherService launcherService, ClientService clientService, CurrentUserState currentUserState) : base(navigationService, config, logger) {
        _serversState = serversState;
        _launcherService = launcherService;
        _clientService = clientService;
        _currentUserState = currentUserState;

        GoToServerListCommand = new MvxCommand(GoToServerList);
        ChooseProximaCommand = new MvxCommand(ChooseProximaServer);
        PlayCommand = new MvxAsyncCommand(PlayAsync);

        LogInfo("[Dashboard] Открытие");
    }

    #endregion

    private async Task PlayAsync() {
        LogInfo("[Start] Старт игры на сервере: {0}", CurrentServerName);
        IsLaunched = true;
        LoadPrecent = 0;
        LoadText = "Загрузка...";

        await PrepareGameAsync();

        StartGame();
    }

    private void GoToServerList() {
        LogInfo("[Dashboard] Переход к списку серверов");
        _serversState.CurrentServerName = string.Empty;
        IsServerList = true;
    }

    private void ChooseProximaServer() {
        _serversState.CurrentServerName = "Proxima";
        CurrentServerUsers = _serversState.GetCurrentServer()?.CurrentUsers ?? 0;
        CurrentServerMaxUsers = _serversState.GetCurrentServer()?.MaxUsers ?? 0;
        IsServerList = false;

        LogInfo("[Dashboard] Выбран сервер: {0}", CurrentServerName);
    }

    private async Task PrepareGameAsync() {
        LogInfo("[Start] Подготовка файлов игры");
        await _launcherService.CheckServerFilesAsync(_serversState.CurrentServerName, DownloadProgressChanged);
        LogInfo("[Start] Все файлы проверены");
    }

    private void DownloadProgressChanged(int percent, string? downloadObject) {
        LoadPrecent = percent;
        LoadText = $"Загрузка клиента: {downloadObject} ...";
    }

    private void StartGame() {
        LogInfo("[Start] Запуск игры");
        LoadPrecent = 100;
        LoadText = "Запуск";
        var settings = new ClientLaunchSettings {
            CurrentUser = _currentUserState.CurrentUser ?? throw new ArgumentNullException("User is empty"),
            MaxRamMb = 4096,
            MinRamMb = 2048
        };

        _clientService.Launch(_serversState.CurrentServerName, settings);
    }
}
