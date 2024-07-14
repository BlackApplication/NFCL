using Models.Json;
using Newtonsoft.Json;
using Serilog;
using Services.Api.Interfaces;
using Services.Helper;
using System.Net;
using System.Text;

namespace Services.Api.Implementations;

public class HttpService : IHttpService {
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    private readonly CookieContainer _cookieContainer;
    private readonly string _startUrl;

    public HttpService(AppConfigModel config, ILogger logger) {
        _cookieContainer = new CookieContainer();
        _httpClient = new HttpClient(new HttpClientHandler { UseCookies = true, CookieContainer = _cookieContainer });
        _startUrl = config.ServerUrl;
        _logger = logger;

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

    public async Task DownloadFileAsync(string url, string path, string parentPath = "", Action<int, string>? downloadProgressChangedAction = null) {
        var request = new HttpRequestMessage(HttpMethod.Get, _startUrl + "/api/" + url);

        using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

        var totalBytes = response.Content.Headers.ContentLength;
        var buffer = new byte[8192];
        var bytesRead = 0L;

        var destinationPath = Path.Combine(parentPath, path);
        using var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None);
        using var contentStream = await response.Content.ReadAsStreamAsync();
        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0) {
            await fileStream.WriteAsync(buffer, 0, (int)bytesRead);

            if (totalBytes.HasValue) {
                var percentComplete = (double)fileStream.Length / totalBytes.Value * 100;
                var objectDownloaded = path.Split('\\')[0];

                downloadProgressChangedAction?.Invoke((int)percentComplete, objectDownloaded);
            }
        }
    }
}
