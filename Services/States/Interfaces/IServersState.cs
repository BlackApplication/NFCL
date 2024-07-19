using Models;

namespace Services.States.Interfaces;

public interface IServersState {
    public string CurrentServerName { get; set; }
    List<ServerInfoModel> Servers { get; set; }
    void Update(ServerInfoModel server);
    ServerInfoModel? GetCurrentServer();
}
