

using Shop.Controls;
using Microsoft.Maui.Controls.Compatibility.Hosting;


namespace Shop;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
         builder.UsePrismApp<App>(PrismStartup.Configure)
                .UseMauiCompatibility()
                .ConfigureMauiHandlers((handlers) =>
                {
#if ANDROID
                    handlers.AddCompatibilityRenderer(typeof(MyEntry), typeof(Shop.Platforms.Android.Services.My_Entry));
                    handlers.AddCompatibilityRenderer(typeof(MySearch), typeof(Shop.Platforms.Android.Services.My_Search));
                    handlers.AddCompatibilityRenderer(typeof(RangeSlider),
                                                      typeof(Shop.Platforms.Android.Services.RangeSlider_Renderer));
                    handlers.AddHandler(typeof(Shop.Controls.Map.Map), typeof(Platforms.Android.Services.Map.MapHandler)); 
#endif
                })

            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Montserrat-ExtraLight.ttf", "Montserrat_Font");
            });

#if ANDROID
        Platforms.Android.Services.Certificate.DangerousAndroidMessageHandlerEmitter.Register();
        Platforms.Android.Services.Certificate.DangerousTrustProvider.Register();
#endif

        builder.Services.AddLocalization();
        builder.ConfigureAnimations();

        return builder.Build();
    }
}
