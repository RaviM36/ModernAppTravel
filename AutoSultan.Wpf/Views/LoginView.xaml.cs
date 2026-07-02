using System.Windows;
using System.Windows.Controls;

namespace AutoSultan.Wpf.Views;

public partial class LoginView : Window
{
    public LoginView(ViewModels.LoginViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is ViewModels.LoginViewModel vm && sender is PasswordBox pb)
        {
            vm.Password = pb.Password;
        }
    }
}