using Models.Api;

namespace Services.Interfaces;

public interface IAuthService {
    public Task<CurrentUserModel?> LoginAsync(LoginModel data);
}
