namespace Models.Json;

public class AppConfigModel {
    public string Version { get; set; } = null!;
    public string ServerUrl { get; set; } = null!;
    public string GameDirectory { get; set; } = null!;
    public DiscordConfig Discord { get; set; } = null!;
}

public class DiscordConfig {
    public string ClientId { get; set; } = null!;
}
