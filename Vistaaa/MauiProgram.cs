using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Maps;

namespace Vistaaa
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Brands-Regular-400.otf", "FAB");
                fonts.AddFont("Free-Regular-400.otf", "FAR");
                fonts.AddFont("Free-Solid-900.otf", "FAS");
                fonts.AddFont("SignikaNegative-Medium.ttf", "SignikaNegative");
            }).UseMauiCommunityToolkit().UseMauiMaps().UseMauiCommunityToolkitMaps("Ak_wMJAB-SJeH0OlnHcRfEN31jFCL4bxutEaEV1L7EYTYGwSu-84TDfWXDLx0xtS");
#if DEBUG
            builder.Logging.AddDebug();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddSingleton<Database>();
#endif
            return builder.Build();
        }
    }
}