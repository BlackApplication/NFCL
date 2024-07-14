using Models;
using Serilog;
using Services.Api.Interfaces;
using Services.Helper;
using System.Diagnostics;

namespace Services;

public class LauncherService {
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
        return await _launcherApi.GetActualVersionAsync();
    }

    public async Task<List<ServerInfoModel>> GetServersListAsync() {
        var model = await _launcherApi.GetServersListAsync();

        return [.. model.Servers];
    } 

    public async Task UpdateLaucnherAsync(Action<int, string?> updateChangedAction) {
        _logger.Information("[Update] Скачивание новой версии");
        await _launcherApi.DownloadActualLauncherAsync(_tempLauncherName, updateChangedAction);
        _logger.Information("[Update] Обновление лаунчера на новый и перезапуск");
        ReplaceAndRestartApplication();

        Environment.Exit(0);
    }

    public async Task CheckServerFilesAsync(string server, Action<int, string> downloadAction) {
        var serverHashesModel = await _launcherApi.GetFilesHashesAsync(server);
        var serverFilesHashes = serverHashesModel.Files;
        var gameDirectory = PathHelper.GetGamePath();
        var localFilesHashes = FileHelper.GetLocalFileHashes(gameDirectory);

        _logger.Information("Проверка сломаных/недостающих файлов");
        var missingFiles = GetMissingFiles(localFilesHashes, serverFilesHashes);
        _logger.Information("Найдено {0} сломаных/недостающих файлов", missingFiles.Count);
        var num = 0;
        var test = (int per, string arg) => {
            num++;
            var percent = num * 100 / missingFiles.Count;
            downloadAction?.Invoke(percent, arg);
        };

        if (missingFiles.Count > 0) {
            await DownloadMissingFilesAsync(server, missingFiles, test);
        }
    }

    private async Task DownloadMissingFilesAsync(string server, List<string> missingFiles, Action<int, string> downloadAction) {
        foreach (var file in missingFiles) {
            var directoriesPath = Path.Combine(PathHelper.GetGamePath(), Path.GetDirectoryName(file));
            if (directoriesPath != string.Empty && directoriesPath != null) {
                Directory.CreateDirectory(directoriesPath);
            }

            _logger.Information("Загрузка файла: {0}", file);
            await _launcherApi.DownloadClientFile(server, file, PathHelper.GetGamePath(), downloadAction);
            _logger.Information("Файл загружен: {0}", file);
        }
    }

    private static List<string> GetMissingFiles(Dictionary<string, string> localFileHashes, Dictionary<string, string> serverFileHashes) {
        var missingFiles = new List<string>();
        foreach (var serverFile in serverFileHashes) {
            if (!localFileHashes.TryGetValue(serverFile.Key, out var localHash) || localHash != serverFile.Value) {
                missingFiles.Add(serverFile.Key);
            }
        }

        return missingFiles;
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
