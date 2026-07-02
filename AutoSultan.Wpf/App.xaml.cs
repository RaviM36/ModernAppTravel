using System;
using System;
using System.Windows;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.EntityFrameworkCore;
using AutoSultan.Wpf.Data;

namespace AutoSultan.Wpf;

public partial class App : Application
{
    private IHost? _host;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Configure Serilog logger
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("autosultan.log")
            .CreateLogger();

        try
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                // Database
                services.AddDbContext<ApplicationDbContext>(opts =>
                    opts.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));

                // Typed HttpClient for REST auth (RestAuthService will receive a configured HttpClient)
                services.AddHttpClient<Services.RestAuthService>(client =>
                {
                    client.BaseAddress = new Uri(context.Configuration["Auth:BaseUrl"] ?? "https://localhost:5001/");
                });

                // Register IAuthService to use REST implementation by default
                services.AddScoped<Services.IAuthService, Services.RestAuthService>();

                // Entra ID service (constructed from configuration values)
                var clientId = context.Configuration["AzureAd:ClientId"] ?? string.Empty;
                var tenantId = context.Configuration["AzureAd:TenantId"] ?? "common";
                var redirect = context.Configuration["AzureAd:RedirectUri"] ?? "http://localhost";
                services.AddSingleton(new Services.EntraAuthService(clientId, tenantId, redirect));

                // ViewModels and Views
                services.AddSingleton<ViewModels.LoginViewModel>();
                // Register LoginView so DI can construct it with IServiceProvider
                services.AddTransient<Views.LoginView>();
                // Dashboard will be created manually with username, no DI registration required
            })
            .Build();

            _host.Start();

            var login = _host.Services.GetRequiredService<Views.LoginView>();
            login.Show();
        }
        catch (Exception ex)
        {
            // Log and show a friendly error so user can see why app failed to start
            try { Serilog.Log.Error(ex, "Failed to start host"); } catch { }
            MessageBox.Show($"Failed to start application: {ex.Message}\nSee autosultan.log for details.", "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
            return;
        }
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (_host is not null)
        {
            await _host.StopAsync();
            _host.Dispose();
            Log.CloseAndFlush();
        }

        base.OnExit(e);
    }
}
