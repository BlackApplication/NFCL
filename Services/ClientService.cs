using CmlLib.Core;
using CmlLib.Core.Auth;
using Models;

namespace Services;

public class ClientService {

    private readonly string _gamePath;

    public ClientService(string gamePath) {
        _gamePath = gamePath;
    }

    public async void Launch(string clientName, ClientLaunchSettings setting) {
        var path = new MinecraftPath(_gamePath);
        var minecraftClient = new CMLauncher(path);

        var options = ProcessSettings(setting);

        var game = await minecraftClient.CreateProcessAsync(clientName, options, checkAndDownload: false);

        AddLogs(minecraftClient);

        game.Start();
    }

    private static void AddLogs(CMLauncher minecraftClient) {
        minecraftClient.FileChanged += (e) => {
            Console.WriteLine("FileKind: " + e.FileKind.ToString());
            Console.WriteLine("FileName: " + e.FileName);
            Console.WriteLine("ProgressedFileCount: " + e.ProgressedFileCount);
            Console.WriteLine("TotalFileCount: " + e.TotalFileCount);
        };
        minecraftClient.ProgressChanged += (s, e) => {
            Console.WriteLine("{0}%", e.ProgressPercentage);
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
