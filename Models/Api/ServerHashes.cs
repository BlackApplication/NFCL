namespace Models.Api;

public class ServerHashes {
    public List<FileHash> Client { get; set; } = null!;
    public List<FileHash> Mods { get; set; } = null!;
}
