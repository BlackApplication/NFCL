using Models;
using Models.Api;
using Models.Enums;
using Serilog;
using Services.Api.Interfaces;
using Services.Helper;
using Services.States.Interfaces;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace Services;

public class LauncherService {
    private readonly string _tempLauncherName = "Temp_Launcher.exe";

    private readonly ILauncherApi _launcherApi;
    private readonly ILogger _logger;
    private readonly IDownloadState _downloadState;

    public LauncherService(ILauncherApi launcherApi, ILogger logger, IDownloadState downloadState) {
        _launcherApi = launcherApi;
        _logger = logger;
        _downloadState = downloadState;
    }

    public async Task<string> GetActualVersionAsync() {
        return await _launcherApi.GetActualVersionAsync();
    }

    public async Task<List<ServerInfoModel>> GetServersListAsync() {
        var model = await _launcherApi.GetServersListAsync();

        return [.. model.Servers];
    } 

    public async Task UpdateLauncherAsync() {
        var launcherSize = await _launcherApi.GetSize();
        _downloadState.Restart(launcherSize);

        _logger.Information("[Update] Скачивание новой версии");
        await _launcherApi.DownloadActualLauncherAsync(_tempLauncherName);

        _logger.Information("[Update] Обновление лаунчера на новый и перезапуск");
        ReplaceAndRestartApplication();

        Environment.Exit(0);
    }

    public async Task CheckServerFilesAsync(string server) {
        var hashesModel = await _launcherApi.GetFilesHashesAsync(server);

        var clientFilesHashes = hashesModel.Client;
        var modsFilesHashes = hashesModel.Mods;
        var localFilesHashes = FileHelper.GetGameLocalFileHashes();

        _logger.Information("[Update] Проверка сломаных/недостающих файлов {0}", "клиента");
        var missingClientFiles = GetMissingFiles(localFilesHashes, clientFilesHashes);
        _logger.Information("[Update] Найдено {0} сломаных/недостающих файлов {1}", missingClientFiles.Count, "клиента");

        _logger.Information("[Update] Проверка сломаных/недостающих файлов {0}", "модов");
        var missingModsFiles = GetMissingFiles(localFilesHashes, modsFilesHashes, "mods");
        _logger.Information("[Update] Найдено {0} сломаных/недостающих файлов {1}", missingModsFiles.Count, "модов");

        var allMissingFiles = missingClientFiles.Concat(missingModsFiles).ToList();
        var allFileHashes = clientFilesHashes.Concat(modsFilesHashes).ToList();
        var allMissingFileHashes = allFileHashes.Where(x => allMissingFiles.Contains(x.Path)).ToList();
        var allBytes = allMissingFileHashes.Select(x => x.Bytes).Sum();
        _downloadState.Restart(allBytes);

        if (missingClientFiles.Count != 0) {
            _logger.Information("[Update] Скачиванием файлов {0}", "клиента");
            await DownloadMissingFilesAsync(server, missingClientFiles, FolderType.Client);
        }

        if (missingModsFiles.Count != 0) {
            _logger.Information("[Update] Скачиванием файлов {0}", "модов");
            await DownloadMissingFilesAsync(server, missingModsFiles, FolderType.Mods);
        }

        if (missingClientFiles.Count == 0 && missingModsFiles.Count == 0) {
            _logger.Information("[Update] Все файлы в порядке!");
        }
    }

    private static List<string> GetMissingFiles(Dictionary<string, string> localFileHashes, List<FileHash> serverFileHashes, string parentPath = "") {
        return serverFileHashes
            .Where(serverFile => !localFileHashes.TryGetValue(Path.Combine(parentPath, serverFile.Path.Replace("/", "\\")), out var localHash) || localHash != serverFile.Hash)
            .Select(serverFile => serverFile.Path)
            .ToList();
    }

    private async Task DownloadMissingFilesAsync(string server, List<string> missingFiles, FolderType folderType) {
        foreach (var file in missingFiles) {
            FileHelper.CheckDirectoriesByFilePath(file);

            var serverFilePath = Path.Combine(folderType.ToString(), file);
            var gamePath = PathHelper.GetGamePath();
            var parentPath = folderType == FolderType.Client ? gamePath : Path.Combine(gamePath, "mods");

            _logger.Information("[Download] Загрузка файла: {0}", file);
            await _launcherApi.DownloadClientFile(server, serverFilePath, parentPath);
            _logger.Information("[Download] Файл загружен: {0}", file);
        }
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
