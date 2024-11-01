using CommunityToolkit.Maui;
using MauiApp1.Classes;
using MauiApp1.Interfaces;
using MauiApp1.Services;
using MauiApp1.ViewModels;

using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;

namespace MauiApp1
{

    public static class MauiProgram
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        /// <summary>  
        /// Creates and configures the Maui application.  
        /// </summary>  
        /// <returns>The configured Maui application.</returns>  
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            // Register services with the dependency injection container  
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddSingleton<ApiRequest>();
            builder.Services.AddSingleton<IRequestManager, RequestManager>();
            builder.Services.AddTransient<LoginPage>(); // Register LoginPage with DI  
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<ProfilePage>(); // Register ProfilePage with DI  
            builder.Services.AddTransient<ProfileViewModel>();
            builder.Services.AddSingleton<HomePage>();

            // Configure the Maui application  
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    // Register fonts with the application  
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseBarcodeReader(); // Use barcode reader functionality  

#if DEBUG
            // Add debug logging in debug mode  
            builder.Logging.AddDebug();
#endif

            // Build and return the configured Maui application  
            var app = builder.Build();
            ServiceProvider = app.Services;
            return app;
        }
    }
}
