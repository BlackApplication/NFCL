using Models.Json;
using Newtonsoft.Json;
using Services.Api.Interfaces;
using Services.Helper;
using System.Net;
using System.Text;

namespace Services.Api.Implementations;

public class HttpService : IHttpService {
    private readonly HttpClient _httpClient;

    private readonly CookieContainer _cookieContainer;
    private readonly string _startUrl;

    public HttpService(AppConfigModel config) {
        _cookieContainer = new CookieContainer();
        _httpClient = new HttpClient(new HttpClientHandler { UseCookies = true, CookieContainer = _cookieContainer });
        _startUrl = config.ServerUrl;

        LoadCookiesFromStorage();
    }

    public async Task<string> GetAsync(string url) {
        var responce = await _httpClient.GetAsync(_startUrl + "/api/" + url);

        return await responce.Content.ReadAsStringAsync();
    }

    public async Task<string> PostAsync<T>(string url, T data) {
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var responce = await _httpClient.PostAsync(_startUrl + "/api/" + url, content);

        return await responce.Content.ReadAsStringAsync();
    }

    public async Task<string> PutAsync<T>(string url, T data) {
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var responce = await _httpClient.PutAsync(_startUrl + "/api/" + url, content);

        return await responce.Content.ReadAsStringAsync();
    }

    public async Task<string> DeleteAsync(string url) {
        var responce = await _httpClient.DeleteAsync(_startUrl + "/api/" + url);

        return await responce.Content.ReadAsStringAsync();
    }

    #region Helpers

    public void SaveCookieToStorage() {
        var cookies = _cookieContainer.GetAllCookies();
        var cookieList = new List<string>();

        foreach (Cookie cookie in cookies) {
            cookieList.Add($"{cookie.Name}={cookie.Value}; Domain={cookie.Domain}; Path={cookie.Path}; Expires={cookie.Expires}");
        }

        var serializedCookies = JsonConvert.SerializeObject(cookieList);
        SecureStorageHelper.SaveData("cookies", serializedCookies);
    }

    public void LoadCookiesFromStorage() {
        var serializedCookies = SecureStorageHelper.LoadData("cookies");
        if (string.IsNullOrEmpty(serializedCookies)) {
            return;
        }

        var cookieList = JsonConvert.DeserializeObject<List<string>>(serializedCookies);
        if (cookieList == null) {
            return;
        }

        foreach (var cookie in cookieList) {
            var cookieParts = cookie.Split(';');
            var nameValue = cookieParts[0].Split('=');
            var path = cookieParts[2].Split('=')[1];
            var domain = cookieParts[1].Split('=')[1];
            var time = cookieParts[3].Split('=')[1];

            var cookieObj = new Cookie(nameValue[0], nameValue[1], path, domain) {
                Expires = DateTime.Parse(time)
            };

            var isExpired = ((DateTimeOffset)cookieObj.Expires).ToUnixTimeSeconds() > DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            if (!isExpired) {
                _cookieContainer.Add(cookieObj);
            }
        }
    }

    #endregion
}
