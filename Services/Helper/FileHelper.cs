using Newtonsoft.Json;
using System.Security.Cryptography;

namespace Services.Helper;

public class FileHelper {
    public static T ReadJsonFile<T>(string path) {
        var json = File.ReadAllText(path);

        return JsonConvert.DeserializeObject<T>(json) ?? throw new Exception("Bad json: " + path + " !");
    }

    public static void CheckDirectoriesByFilePath(string filePath) {
        var directoryPath = Path.GetDirectoryName(filePath);

        CheckDirectoriesByPath(directoryPath ?? "");
    }

    public static void CheckDirectoriesByPath(string directoryPath) {
        if (directoryPath != string.Empty && directoryPath != null) {
            var fullPath = Path.Combine(PathHelper.GetGamePath(), directoryPath);

            Directory.CreateDirectory(fullPath);
        }
    }

    public static Dictionary<string, string> GetGameLocalFileHashes() {
        var fileHashes = new Dictionary<string, string>();
        var gameDirectory = PathHelper.GetGamePath();
        foreach (var filePath in Directory.GetFiles(gameDirectory, "*", SearchOption.AllDirectories)) {
            var relativePath = Path.GetRelativePath(gameDirectory, filePath);
            var hash = ComputeHashFromFile(filePath);
            fileHashes[relativePath] = hash;
        }

        return fileHashes;
    }

    public static string ComputeHashFromFile(string filePath) {
        using var fileStream = File.OpenRead(filePath);

        return ComputeHashFromStream(fileStream);
    }

    public static string ComputeHashFromStream(Stream stream) {
        using var md5 = MD5.Create();
        stream.Position = 0;

        var hashBytes = md5.ComputeHash(stream);

        return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
    }

    public static bool CompareFileHashes(string filePath, Stream stream) {
        var fileHash = ComputeHashFromFile(filePath);
        var streamHash = ComputeHashFromStream(stream);

        return fileHash == streamHash;
    }

    public static bool CompareFileHashes(Stream firstStream, Stream secondStream) {
        var firstStreamHash = ComputeHashFromStream(firstStream);
        var secondStreamHash = ComputeHashFromStream(secondStream);

        return firstStreamHash == secondStreamHash;
    }

    public static bool CompareFileHashes(string firstFilePath, string secondFilePath) {
        var firstFileHash = ComputeHashFromFile(firstFilePath);
        var secondFileHash = ComputeHashFromFile(secondFilePath);

        return firstFileHash == secondFileHash;
    }
}
