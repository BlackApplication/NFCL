using Newtonsoft.Json;
using System.IO;

namespace Core.Configuration;

public class ConfigLoader {
    public static AppConfig Load(string path) {
        var configJson = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<AppConfig>(configJson) ?? throw new Exception("Bad configuration!");
    }
}
