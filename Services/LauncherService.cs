using Models;
using Serilog;
using Services.Api.Interfaces;
using System.Diagnostics;

namespace Services;

public class LauncherService {
    public event Action<int> UpdateProgressChanged = null!;
    private readonly string _launcherDirectory;
    private readonly string _tempLauncherName = "Temp_Launcher.exe";

    private readonly ILauncherApi _launcherApi;
    private readonly ILogger _logger;

    public LauncherService(string laucnherDirectory, ILauncherApi launcherApi, ILogger logger) {
        _launcherDirectory = laucnherDirectory;
        _launcherApi = launcherApi;
        _logger = logger;
    }

    public async Task<string> GetActualVersionAsync() {
        return await _launcherApi.GetActualVersion();
    }

    public async Task<List<ServerInfoModel>> GetServersListAsync() {
        var model = await _launcherApi.GetServersList();

        return [.. model.Servers];
    }

    public async Task UpdateLaucnherAsync() {
        _logger.Information("[Update] Скачивание новой версии");
        await _launcherApi.DownloadActualLauncher(_tempLauncherName, UpdateProgressChanged);
        _logger.Information("[Update] Обновление лаунчера на новый и перезапуск");
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
