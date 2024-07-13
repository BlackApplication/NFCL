using Models.Api;

namespace Services.Api.Interfaces;

public interface ILauncherApi {
    public Task<string> GetActualVersion();
    public Task<ServersList> GetServersList();
    Task DownloadActualLauncher(string destinationPath);
}
