using System.Windows;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace AutoSultan.Wpf.Views;

public partial class LoginView : Window
{
    private readonly IServiceProvider _services;

    public LoginView(ViewModels.LoginViewModel vm, IServiceProvider services)
    {
        InitializeComponent();
        DataContext = vm;
        _services = services;

        // subscribe to login success
        vm.LoginSucceeded += OnLoginSucceeded;
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is ViewModels.LoginViewModel vm && sender is PasswordBox pb)
        {
            vm.Password = pb.Password;
        }
    }

    private void OnLoginSucceeded(string? username)
    {
        Dispatcher.Invoke(() =>
        {
            var dashboard = new DashboardView(username ?? string.Empty, _services);
            dashboard.Show();
            Close();
        });
    }

    private const int WM_SETICON = 0x0080;
    private static readonly IntPtr ICON_SMALL = new IntPtr(0);
    private static readonly IntPtr ICON_BIG = new IntPtr(1);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        try
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SendMessage(hwnd, WM_SETICON, ICON_SMALL, IntPtr.Zero);
            SendMessage(hwnd, WM_SETICON, ICON_BIG, IntPtr.Zero);
        }
        catch { }
    }
}
