using Core.States;
using Core.ViewModels.Base;
using Models.Json;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Serilog;

namespace Core.ViewModels;

public class DashboardViewModel : BaseViewModel {

    #region Services

    private readonly ServersState _serversState;

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

    public MvxCommand PlayCommand { get; }

    #endregion

    #region Constructor

    public DashboardViewModel(IMvxNavigationService navigationService, AppConfigModel config, ILogger logger, ServersState serversState) : base(navigationService, config, logger) {
        _serversState = serversState;

        GoToServerListCommand = new MvxCommand(GoToServerList);
        ChooseProximaCommand = new MvxCommand(ChooseProximaServer);
        PlayCommand = new MvxCommand(Play);

        LogInfo("[Dashboard] Открытие");
    }

    #endregion

    private void Play() {
        LogInfo("[Dashboard] Старт игры на сервере: {0}", CurrentServerName);
        IsLaunched = true;
        LoadPrecent = 60;
        LoadText = "Загрузка...";
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
}
