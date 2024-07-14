using Models.Api;

namespace Services.Api.Interfaces;

public interface ILauncherApi {
    Task<string> GetActualVersionAsync();
    Task<ServersList> GetServersListAsync();
    Task DownloadActualLauncherAsync(string destinationPath, Action<int, string?> updateChangedAction);
    Task<ServerHashes> GetFilesHashesAsync(string server);
    Task DownloadClientFile(string server, string path, string parentPath = "", Action<int, string>? downloadAction = null);
}
