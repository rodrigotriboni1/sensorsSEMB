using Microsoft.Extensions.Logging;
using OxyPlot.Maui.Skia;
using SkiaSharp.Views.Maui.Controls;
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
                .UseOxyPlotSkia()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            // Adiciona logging para debug no ambiente de desenvolvimento
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
