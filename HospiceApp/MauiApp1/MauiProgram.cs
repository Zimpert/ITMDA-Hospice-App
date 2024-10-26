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
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.Services.AddSingleton<AbstractRequest>();
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddSingleton<IRequestManager, RequestManager>();
            builder.Services.AddTransient<LoginViewModel>();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseBarcodeReader();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
