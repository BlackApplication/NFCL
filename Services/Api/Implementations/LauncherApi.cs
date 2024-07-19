using Models.Api;
using Newtonsoft.Json;
using Services.Api.Interfaces;
using System.Text.RegularExpressions;

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

    public async Task<long> GetSize() {
        var result = await _httpService.GetAsync("Launcher/GetSize");

        return long.Parse(result);
    }

    public async Task DownloadActualLauncherAsync(string tempName) {
        await _httpService.DownloadFileAsync("Launcher/Download", tempName);
    }

    public async Task<ServerHashes> GetFilesHashesAsync(string server) {
        var result =  await _httpService.GetAsync($"Launcher/GetFilesHashes/{server}");

        return JsonConvert.DeserializeObject<ServerHashes>(result) ?? throw new Exception("Get servers list error!");
    }

    public async Task DownloadClientFile(string server, string path, string parentPath) {
        var pathWithRemovedFolderType = Regex.Replace(path, @"^(Client|Mods)\\", "");
        var downloadPath = Path.Combine(parentPath, pathWithRemovedFolderType);

        await _httpService.DownloadFileAsync($"Launcher/DownloadClientFile/{server}/{Uri.EscapeDataString(path)}", downloadPath);
    }
}
