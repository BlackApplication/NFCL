using System.Diagnostics;

namespace Services.Helper;

public static class BrowserHelper {
    public static void OpenBrowser(string url) {
        try {
            Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
        } catch (Exception) { }
    }
}
