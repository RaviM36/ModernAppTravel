using System.ComponentModel;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

using AutoSultan.Wpf.Services;

namespace AutoSultan.Wpf.ViewModels;

public class LoginViewModel : INotifyPropertyChanged
{
    private readonly IAuthService _authService;
    private readonly EntraAuthService? _entra;
    private string? _username;
    private string? _password;

    // Default test credentials
    private const string DefaultUsername = "demo@autosultan.local";
    private const string DefaultPassword = "Demo@1234";

    public LoginViewModel(IAuthService authService, EntraAuthService? entra = null)
    {
        _authService = authService;
        _entra = entra;
        LoginCommand = new Helpers.RelayCommand(async _ => await LoginAsync(), _ => true);
        CloseCommand = new Helpers.RelayCommand(_ => CloseWindow(), _ => true);
        EntraSignInCommand = new Helpers.RelayCommand(async _ => await EntraSignInAsync(), _ => _entra != null);
    }

    public string? Username
    {
        get => _username;
        set { _username = value; OnPropertyChanged(); }
    }

    public string? Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); }
    }

    public System.Windows.Input.ICommand LoginCommand { get; }
    public System.Windows.Input.ICommand CloseCommand { get; }
    public System.Windows.Input.ICommand EntraSignInCommand { get; }

    // Raised when login succeeds. Payload: username
    public event Action<string?>? LoginSucceeded;

    private async Task LoginAsync()
    {
        // Accept default test credentials without backend
        if (string.Equals(Username, DefaultUsername, StringComparison.OrdinalIgnoreCase)
            && string.Equals(Password, DefaultPassword, StringComparison.Ordinal))
        {
            MessageBox.Show("Login successful (default test user)", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            LoginSucceeded?.Invoke(Username);
            return;
        }

        var ok = await _authService.AuthenticateAsync(Username ?? string.Empty, Password ?? string.Empty);
        if (ok)
        {
            MessageBox.Show("Login successful", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            LoginSucceeded?.Invoke(Username);
        }
        else
        {
            MessageBox.Show("Invalid credentials", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task EntraSignInAsync()
    {
        if (_entra is null)
        {
            MessageBox.Show("Entra ID not configured", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var result = await _entra.SignInInteractiveAsync();
        if (result is not null)
        {
            Username = result.Account.Username;
            MessageBox.Show("Signed in with Entra ID", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void CloseWindow()
    {
        Application.Current?.Shutdown();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}