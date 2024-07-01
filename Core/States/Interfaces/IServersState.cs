using Models;

namespace Core.States.Interfaces;

public interface IServersState {
    public void Update(ServerInfoModel server);
}
