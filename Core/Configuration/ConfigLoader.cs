using Newtonsoft.Json;

namespace Core.Configuration;

public class ConfigLoader {
    public static AppConfigModel Load(string path) {
        var configJson = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<AppConfigModel>(configJson) ?? throw new Exception("Bad configuration!");
    }
}
