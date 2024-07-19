using Newtonsoft.Json;
using System.Net;

namespace Services.Helper;

public static class CookieHelper {
    public static void SaveCookieToStorage(CookieContainer container) {
        var cookies = container.GetAllCookies();
        var cookieList = new List<string>();

        foreach (Cookie cookie in cookies) {
            cookieList.Add($"{cookie.Name}={cookie.Value}; Domain={cookie.Domain}; Path={cookie.Path}; Expires={cookie.Expires}");
        }

        var serializedCookies = JsonConvert.SerializeObject(cookieList);
        SecureStorageHelper.SaveData("cookies", serializedCookies);
    }

    public static void LoadCookiesFromStorage(CookieContainer container) {
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

            var expiredTime = new DateTimeOffset(cookieObj.Expires, TimeSpan.Zero);
            var expiredTimeOnSecounds = expiredTime.ToUnixTimeSeconds();
            var currentTimeOnSecounds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var isExpired = currentTimeOnSecounds > expiredTimeOnSecounds;
            if (!isExpired) {
                container.Add(cookieObj);
            }
        }
    }
}
