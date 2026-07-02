using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoSultan.Wpf.Models;

namespace AutoSultan.Wpf.Services;

public class RestAuthService : IAuthService
{
    private readonly HttpClient _client;

    public RestAuthService(HttpClient client)
    {
        _client = client;
    }

    public async Task<bool> AuthenticateAsync(string username, string password)
    {
        var creds = new { Username = username, Password = password };
        try
        {
            var resp = await _client.PostAsJsonAsync("api/auth/login", creds);
            if (!resp.IsSuccessStatusCode) return false;

            // Expect a simple { success: true } or token response
            var obj = await resp.Content.ReadFromJsonAsync<LoginResponse?>();
            return obj is not null && obj.Success;
        }
        catch
        {
            return false;
        }
    }

    private record LoginResponse(bool Success, string? Token);
}
