using Models.Api;
using Newtonsoft.Json;
using Services.Api.Interfaces;

namespace Services.Api.Implementations;

public class LauncherApi : ILauncherApi {
    private readonly IHttpService _httpService;

    public LauncherApi(IHttpService httpService) {
        _httpService = httpService;
    }

    public async Task<string> GetActualVersionAsync() {
        return await _httpService.GetAsync("Launcher/GetActualVersion");
    }

    public async Task<ServersList> GetServersListAsync() {
        var result = await _httpService.GetAsync("Launcher/GetServersList");

        return JsonConvert.DeserializeObject<ServersList>(result) ?? throw new Exception("Get servers list error!");
    }

    public async Task DownloadActualLauncherAsync(string path, Action<int, string> updateChangedAction) {
        await _httpService.DownloadFileAsync("Launcher/Download", path, "", updateChangedAction);
    }

    public async Task<ServerHashes> GetFilesHashesAsync(string server) {
        var result =  await _httpService.GetAsync($"Launcher/GetFilesHashes/{server}");

        return JsonConvert.DeserializeObject<ServerHashes>(result) ?? throw new Exception("Get servers list error!");
    }

    public async Task DownloadClientFile(string server, string path, string parentPath = "", Action<int, string>? downloadAction = null) {
        await _httpService.DownloadFileAsync($"Launcher/DownloadClientFile/{server}/{Uri.EscapeDataString(path)}", path, parentPath, downloadAction);
    }
}
