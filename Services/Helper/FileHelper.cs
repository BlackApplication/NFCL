using Newtonsoft.Json;

namespace Services.Helper;

public class FileHelper {
    public static T ReadJsonFile<T>(string path) {
        var json = File.ReadAllText(path);

        return JsonConvert.DeserializeObject<T>(json) ?? throw new Exception("Bad json: " + path + " !");
    }
}
