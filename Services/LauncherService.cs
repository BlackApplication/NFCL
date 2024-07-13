using Models;
using Services.Api.Interfaces;
using System.Diagnostics;

namespace Services;

public class LauncherService {
    private readonly string _laucnherDirectory;
    private readonly string _tempLauncherName = "Temp_Launcher.exe";

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

    public async Task UpdateLaucnherAsync() {
        await _launcherApi.DownloadActualLauncher(_tempLauncherName);
        ReplaceAndRestartApplication();

        Environment.Exit(0);
    }

    private void ReplaceAndRestartApplication() {
        var oldFilePath = "Launcher.exe";

        var currentProcessId = Environment.ProcessId;
        var startInfo = new ProcessStartInfo {
            FileName = "cmd.exe",
            Arguments = $"/C timeout 2 & taskkill /PID {currentProcessId} & move /Y \"{_tempLauncherName}\" \"{oldFilePath}\" & \"{oldFilePath}\"",
            WindowStyle = ProcessWindowStyle.Hidden
        };

        Process.Start(startInfo);
    }
}
