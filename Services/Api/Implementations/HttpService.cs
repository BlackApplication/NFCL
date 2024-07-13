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

        CookieHelper.LoadCookiesFromStorage(_cookieContainer);
    }

    public async Task<string> GetAsync(string url) {
        var responce = await _httpClient.GetAsync(_startUrl + "/api/" + url);
        CookieHelper.SaveCookieToStorage(_cookieContainer);

        return await responce.Content.ReadAsStringAsync();
    }

    public async Task<string> PostAsync<T>(string url, T data) {
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var responce = await _httpClient.PostAsync(_startUrl + "/api/" + url, content);
        CookieHelper.SaveCookieToStorage(_cookieContainer);

        return await responce.Content.ReadAsStringAsync();
    }

    public async Task<string> PutAsync<T>(string url, T data) {
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var responce = await _httpClient.PutAsync(_startUrl + "/api/" + url, content);
        CookieHelper.SaveCookieToStorage(_cookieContainer);

        return await responce.Content.ReadAsStringAsync();
    }

    public async Task<string> DeleteAsync(string url) {
        var responce = await _httpClient.DeleteAsync(_startUrl + "/api/" + url);
        CookieHelper.SaveCookieToStorage(_cookieContainer);

        return await responce.Content.ReadAsStringAsync();
    }

    public async Task DownloadFileAsync(string url, string destinationPath) {
        var request = new HttpRequestMessage(HttpMethod.Get, _startUrl + "/api/" + url);

        using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        using var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None);
        await response.Content.CopyToAsync(fileStream);
    }
}
