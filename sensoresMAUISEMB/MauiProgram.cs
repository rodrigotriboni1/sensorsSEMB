using Material.Components.Maui.Extensions;
using Microsoft.Extensions.Logging;
using OxyPlot.Maui.Skia;
using SkiaSharp.Views.Maui.Controls;
using SkiaSharp.Views.Maui.Controls.Compatibility;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace sensoresMAUISEMB
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .UseMaterialComponents() // material m3
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //builder.Services.AddHttpClient("api", httpClient => httpClient.BaseAddress = new Uri("http://localhost:5257/WeatherForecast");

#if DEBUG
            // Adiciona logging para debug no ambiente de desenvolvimento
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
