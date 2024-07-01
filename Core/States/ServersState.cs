using Core.States.Interfaces;
using Models;
using Services.Helper;

namespace Core.States;

public class ServersState : IServersState {
    private string _currentServerName = string.Empty;
    public string CurrentServerName {
        get => _currentServerName;
        set => _currentServerName = value;
    }

    private List<ServerInfoModel> _servers = [];
    public List<ServerInfoModel> Servers {
        get => _servers;
        set => _servers = value;
    }

    public void Update(ServerInfoModel server) {
        var serverToUpdate = Servers.Find(x => x.Name == server.Name);
        if (serverToUpdate != null) {
            return;
        }

        ObjectHelper.UpdateProperties(serverToUpdate, server);
    }

    public ServerInfoModel? GetCurrentServer() {
        return Servers.Find(x => x.Name == CurrentServerName) ?? null;
    }
}
