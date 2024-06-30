namespace Models;

public class ServerInfoModel {
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public int Port { get; set; }
    public string Version { get; set; } = null!;
    public bool IsOnline { get; set; }
    public int? CurrentUsers { get; set; }
    public int? MaxUsers { get; set; }
}
