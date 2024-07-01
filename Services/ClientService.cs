using CmlLib.Core;
using CmlLib.Core.Auth;
using Models;
using Serilog;

namespace Services;

public class ClientService {

    private readonly ILogger _logger;

    private readonly string _gamePath;

    public ClientService(string gamePath, ILogger logger) {
        _gamePath = gamePath;
        _logger = logger;
    }

    public async void Launch(string clientName, ClientLaunchSettings setting) {
        var path = new MinecraftPath(_gamePath);
        var minecraftClient = new CMLauncher(path);

        var options = ProcessSettings(setting);

        var game = await minecraftClient.CreateProcessAsync(clientName, options, checkAndDownload: false);

        AddLogs(minecraftClient);

        game.Start();
    }

    private void AddLogs(CMLauncher minecraftClient) {
        minecraftClient.FileChanged += (e) => {
            _logger.Information("FileKind: " + e.FileKind.ToString());
            _logger.Information("FileName: " + e.FileName);
            _logger.Information("ProgressedFileCount: " + e.ProgressedFileCount);
            _logger.Information("TotalFileCount: " + e.TotalFileCount);
        };
        minecraftClient.ProgressChanged += (s, e) => {
            _logger.Information("{0}%", e.ProgressPercentage);
        };
    }

    private static MLaunchOption ProcessSettings(ClientLaunchSettings settings) {
        var session = MSession.CreateOfflineSession(settings.CurrentUser.Name);
        session.ClientToken = settings.CurrentUser.ClientToken.ToString();

        return new MLaunchOption {
            Session = session,
            MaximumRamMb = settings.MaxRamMb
        };
    }
}
