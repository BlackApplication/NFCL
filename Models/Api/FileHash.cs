namespace Models.Api;

public class FileHash {
    public string Path { get; set; } = null!;
    public string Hash { get; set; } = null!;
    public long Bytes { get; set; }
}
