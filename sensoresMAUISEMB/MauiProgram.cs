﻿using Material.Components.Maui.Extensions;
using Microsoft.Extensions.Logging;
using OxyPlot.Maui.Skia;
using SkiaSharp.Views.Maui.Controls;
using SkiaSharp.Views.Maui.Controls.Compatibility;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Syncfusion.Maui.Core.Hosting;
using Microsoft.Extensions.DependencyInjection;


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
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            var apiUrl = "http://192.168.56.1:5257/api/sensors";

            // Register HttpClient
            builder.Services.AddHttpClient("api", httpClient =>
            {
                httpClient.BaseAddress = new Uri(apiUrl);
            });

            builder.Services.AddSingleton<SensorManager>();

#if DEBUG
            // Adiciona logging para debug no ambiente de desenvolvimento
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
