using Core.Configuration;
using Newtonsoft.Json;
using Services.Interfaces;
using System.Text;

namespace Services.Implementations;

public class HttpService : IHttpService {
    private readonly HttpClient _httpClient;

    private readonly string _startUrl;

    public HttpService(AppConfigModel config) {
        _httpClient = new HttpClient();
        _startUrl = config.ServerUrl;
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
}
