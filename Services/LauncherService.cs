using Models;
using Models.Enums;
using Services.Api.Interfaces;

namespace Services;

public class LauncherService {
    private readonly string _laucnherDirectory;
    private readonly ILauncherApi _launcherApi;

    public LauncherService(string laucnherDirectory, ILauncherApi launcherApi) {
        _laucnherDirectory = laucnherDirectory;
        _launcherApi = launcherApi;
    }

    public async Task<string> GetActualVersionAsync() {
        return await _launcherApi.GetActualVersion();
    }

    public async Task<List<ServerInfoModel>> GetServersListAsync() {
        var model = await _launcherApi.GetServersList();

        return [.. model.Servers];
    }

    public Task UpdateLaucnher(SystemType system, string laucnherName, string currentVersion) {
        throw new NotImplementedException();
    }

    public void LauncherReplaceAndRestart(SystemType system, string laucnherName) {
        throw new NotImplementedException();
    }
}
