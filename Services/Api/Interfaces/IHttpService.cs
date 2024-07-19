namespace Services.Api.Interfaces;

public interface IHttpService
{
    Task<string> GetAsync(string url);
    Task<string> PostAsync<T>(string url, T data);
    Task<string> PutAsync<T>(string url, T data);
    Task<string> DeleteAsync(string url);
    Task DownloadFileAsync(string url, string savePath);
}
