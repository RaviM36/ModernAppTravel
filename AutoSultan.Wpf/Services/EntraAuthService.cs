using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace AutoSultan.Wpf.Services;

public class EntraAuthService
{
    private readonly IPublicClientApplication _app;
    private readonly string[] _scopes = new[] { "User.Read" };

    public EntraAuthService(string clientId, string tenantId, string redirectUri)
    {
        _app = PublicClientApplicationBuilder
            .Create(clientId)
            .WithAuthority(AzureCloudInstance.AzurePublic, tenantId)
            .WithRedirectUri(redirectUri)
            .Build();
    }

    public async Task<AuthenticationResult?> SignInInteractiveAsync()
    {
        try
        {
            var accounts = await _app.GetAccountsAsync();
            return await _app.AcquireTokenInteractive(_scopes).ExecuteAsync();
        }
        catch
        {
            return null;
        }
    }
}
