using Models.Api;

namespace Models;

public class ClientLaunchSettings {
    public int MaxRamMb { get; set; }
    public int MinRamMb { get; set; }
    public CurrentUserModel CurrentUser { get; set; } = null!;

}
