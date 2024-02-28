using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Camera.UI.Models;

public class ServerResponse<T>
{
    public static async Task<ServerResponse<T>> CreateFromJson(HttpResponseMessage message)
    {
        var res = new ServerResponse<T>()
        {
            IsSuccess = message.IsSuccessStatusCode,
            Data = message.IsSuccessStatusCode ? await message.Content.ReadFromJsonAsync<T>() : default,
            Error = message.IsSuccessStatusCode ? string.Empty : await message.Content.ReadAsStringAsync(),
        };
        return res;
    }

    public static async Task<ServerResponse<string>> CreateFromString(HttpResponseMessage message)
    {
        var res = new ServerResponse<string>()
        {
            IsSuccess = message.IsSuccessStatusCode,
            Data = message.IsSuccessStatusCode ? await message.Content.ReadAsStringAsync() : default,
            Error = message.IsSuccessStatusCode ? string.Empty : await message.Content.ReadAsStringAsync(),
        };
        return res;
    }

    public static AccesToken ParseToken(string data)
    {
        return JsonSerializer.Deserialize<AccesToken>(data);
    }

    public bool IsSuccess { get; set; }

    public T? Data { get; set; }

    public string Error { get; set; } = string.Empty;

}
public class AccesToken
{
    public string access_token { get; set; }
    public string username { get; set; }
    public string expires { get; set; }
}
