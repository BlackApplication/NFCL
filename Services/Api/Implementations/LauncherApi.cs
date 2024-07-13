using Models.Api;
using Newtonsoft.Json;
using Services.Api.Interfaces;

namespace Services.Api.Implementations;

public class LauncherApi : ILauncherApi {
    private readonly IHttpService _httpService;

    public LauncherApi(IHttpService httpService) {
        _httpService = httpService;
    }

    public Task<string> GetActualVersion() {
        return _httpService.GetAsync("Launcher/GetActualVersion");
    }

    public async Task<ServersList> GetServersList() {
        var result = await _httpService.GetAsync("Launcher/GetServersList");

        return JsonConvert.DeserializeObject<ServersList>(result) ?? throw new Exception("Get servers list error!");
    }

    public async Task DownloadActualLauncher(string destinationPath) {

        await _httpService.DownloadFileAsync("Launcher/Download", destinationPath);
    }
}
