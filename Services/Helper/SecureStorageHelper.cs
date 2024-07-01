using System.Security.Cryptography;
using System.Text;

namespace Services.Helper;

public static class SecureStorageHelper {
    public static void SaveData(string key, string data) {
        var encryptedData = Protect(Encoding.UTF8.GetBytes(data));

        File.WriteAllBytes(PathHelper.GetGameFilePath(key), encryptedData);
    }

    public static string LoadData(string key) {
        if (!File.Exists(PathHelper.GetGameFilePath(key)))
            return string.Empty;

        var encryptedData = File.ReadAllBytes(PathHelper.GetGameFilePath(key));
        var decryptedData = Unprotect(encryptedData);
        return Encoding.UTF8.GetString(decryptedData);
    }

    private static string GetFilePath(string key) => $"{key}.dat";

    private static byte[] Protect(byte[] data) {
        return ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
    }

    private static byte[] Unprotect(byte[] data) {
        return ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
    }
}
