using Models.Json;

namespace Services.Helper;

public static class PathHelper {
    public static string GetGameDirectory() {
        return FileHelper.ReadJsonFile<AppConfigModel>("Configuration/config.json").GameDirectory;
    }

    public static string GetGamePath() {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), GetGameDirectory());
    }

    public static string GetCachePath() {
        return Path.Combine(GetGamePath(), "cache.json");
    }
}
