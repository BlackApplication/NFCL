using Models.Api;

namespace Services.Api.Interfaces;

public interface IAuthService
{
    public Task<CurrentUserModel?> LoginAsync(LoginModel data);
    public Task<CurrentUserModel> GetCurrentUserAsync();
}
