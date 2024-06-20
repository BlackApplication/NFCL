using Models.Api;
using Newtonsoft.Json;
using Services.Interfaces;

namespace Services.Implementations;

public class AuthService : IAuthService {
    private readonly IHttpService _httpService;

    public AuthService(IHttpService httpService) {
        _httpService = httpService;
    }

    public async Task<CurrentUserModel?> LoginAsync(LoginModel data) {
        var jsonResult = await _httpService.PostAsync("Auth/Login", data);
        if (jsonResult == "Invalid credentials!") {
            return null;
        }

        return JsonConvert.DeserializeObject<CurrentUserModel>(jsonResult);
    }
}
