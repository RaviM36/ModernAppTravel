using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace AutoSultan.Wpf.ViewModels;

public class DashboardViewModel : INotifyPropertyChanged
{
    private readonly DispatcherTimer _timer;
    private string _currentDateTime;

    public DashboardViewModel(string username)
    {
        Username = username;
        _currentDateTime = DateTime.Now.ToString("F");

        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += (s, e) => CurrentDateTime = DateTime.Now.ToString("F");
        _timer.Start();

        LogoutCommand = new Helpers.RelayCommand(_ => { LogoutRequested?.Invoke(); }, _ => true);
    }

    public string Username { get; }
    public string CurrentDateTime
    {
        get => _currentDateTime;
        set { _currentDateTime = value; OnPropertyChanged(); }
    }

    public System.Windows.Input.ICommand LogoutCommand { get; }
    public event Action? LogoutRequested;

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}