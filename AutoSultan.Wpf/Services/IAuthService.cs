using System.Threading.Tasks;

namespace AutoSultan.Wpf.Services;

public interface IAuthService
{
    Task<bool> AuthenticateAsync(string username, string password);
}