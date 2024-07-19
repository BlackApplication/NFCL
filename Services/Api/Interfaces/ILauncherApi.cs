using Models.Api;

namespace Services.Api.Interfaces;

public interface ILauncherApi {
    Task<string> GetActualVersionAsync();
    Task<ServersList> GetServersListAsync();
    Task<long> GetSize();
    Task DownloadActualLauncherAsync(string tempName);
    Task<ServerHashes> GetFilesHashesAsync(string server);
    Task DownloadClientFile(string server, string path, string parentPath = "");
}
