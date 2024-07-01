using Core.States;
using Core.ViewModels.Base;
using Models.Json;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Serilog;
using Services.Helper;

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

    public string CurrentServerName => TextHelper.CapitalText(_serversState.CurrentServerName);

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
    }

    #endregion

    private void Play() {
        IsLaunched = true;
        RaisePropertyChanged(() => ProgresBarVisible);
        RaisePropertyChanged(() => PlayButtonVisible);
        LoadPrecent = 60;
        LoadText = "Загрузка...";
        RaisePropertyChanged(() => LoadPrecent);
        RaisePropertyChanged(() => LoadText);
    }

    private void GoToServerList() {
        _serversState.CurrentServerName = string.Empty;
        IsServerList = true;

        RaiseAllPropertiesChanged();
    }

    private void ChooseProximaServer() {
        _serversState.CurrentServerName = "proxima";
        IsServerList = false;
        CurrentServerUsers = _serversState.GetCurrentServer()?.CurrentUsers ?? 0;
        CurrentServerMaxUsers = _serversState.GetCurrentServer()?.MaxUsers ?? 0;

        RaiseAllPropertiesChanged();
    }
}
