namespace Services.Helper;

public static class PathHelper {
    public static string GetGamePath() {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".nightfallcraft");
    }

    public static string GetGameFilePath(string file) {
        return Path.Combine(GetGamePath(), file);
    }
}
