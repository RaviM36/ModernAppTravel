using System.Threading.Tasks;

namespace AutoSultan.Wpf.Services;

public class AuthService : IAuthService
{
    // Stub implementation. Replace with REST / Microsoft Identity flow.
    public Task<bool> AuthenticateAsync(string username, string password)
    {
        // For demo: accept any non-empty username/password
        var ok = !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password);
        return Task.FromResult(ok);
    }
}