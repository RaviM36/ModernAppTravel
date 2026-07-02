using System;
using System.Windows;

namespace AutoSultan.Wpf.Views;

public partial class DashboardView : Window
{
    private readonly IServiceProvider _services;

    public DashboardView(string username, IServiceProvider services)
    {
        InitializeComponent();
        _services = services;
        var vm = new ViewModels.DashboardViewModel(username);
        vm.LogoutRequested += OnLogoutRequested;
        DataContext = vm;
    }

    private void OnLogoutRequested()
    {
        Dispatcher.Invoke(() =>
        {
            // Resolve login view via DI if possible
            var login = _services.GetService(typeof(Views.LoginView)) as Window;
            if (login == null)
            {
                // Fallback: try to create by activating with service provider
                try
                {
                    login = Activator.CreateInstance(typeof(Views.LoginView), _services.GetService(typeof(ViewModels.LoginViewModel)), _services) as Window;
                }
                catch { }
            }

            login?.Show();
            Close();
        });
    }
}
