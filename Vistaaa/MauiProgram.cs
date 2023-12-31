﻿using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Maps;
using Vistaaa.Views;
using Vistaaa.ViewModel;

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


#if WINDOWS
            Microsoft.Maui.Handlers.DatePickerHandler.Mapper.Add(nameof(View.HorizontalOptions), MapHorizontalOptions);
            
            Microsoft.Maui.Handlers.SwitchHandler.Mapper.AppendToMapping("NoLabel", (handler, view) =>
            {
                    handler.PlatformView.OnContent = null;
                    handler.PlatformView.OffContent = null;
                    handler.PlatformView.MinWidth = 0;
            });
#endif

            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<OffersPage>();
            builder.Services.AddSingleton<ProfilePage>();
            builder.Services.AddSingleton<Database>();

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegistrationPage>();
            builder.Services.AddTransient<RegistrationViewModel>();
            builder.Services.AddTransient<AdvertisementPage>();
            builder.Services.AddTransient<AddOrEditAdvertisement>();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
#if WINDOWS
        private static void MapHorizontalOptions(IViewHandler handler, IView view)
        {
            if (view is not View mauiView)
            {
                return;
            }

            if (handler.PlatformView is not Microsoft.UI.Xaml.FrameworkElement element)
            {
                return;
            }

            element.HorizontalAlignment = mauiView.HorizontalOptions.Alignment switch
            {
                LayoutAlignment.Start => Microsoft.UI.Xaml.HorizontalAlignment.Left,
                LayoutAlignment.Center => Microsoft.UI.Xaml.HorizontalAlignment.Center,
                LayoutAlignment.End => Microsoft.UI.Xaml.HorizontalAlignment.Right,
                LayoutAlignment.Fill => Microsoft.UI.Xaml.HorizontalAlignment.Stretch,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
#endif
    }
}