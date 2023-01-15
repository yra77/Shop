

using Shop.Services.SettingsManager;
using Shop.Services.VerifyService;
using Shop.Services.Db_Products;
using Shop.Services.HttpService;
using Shop.Services.Interfaces;
using Shop.Services.Repository;
using Shop.Services.SendEmail;
using Shop.Services.Db_Cart;
using Shop.Services.Auth;
using Shop.ViewModels;
using Shop.Views;


namespace Shop;

internal static class PrismStartup
{
    public static void Configure(PrismAppBuilder builder)
    {
        builder.RegisterTypes(RegisterTypes)
                .OnAppStart("MainPage");
    }

    private static void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>()
                         .RegisterForNavigation<SignUp, SignUpViewModel>()
                         .RegisterForNavigation<RecoveryPassword, RecoveryPasswordViewModel>()
                         .RegisterForNavigation<ShopMain, ShopMainViewModel>()
                         .RegisterForNavigation<FilterModal, FilterModalViewModel>()
                         .RegisterForNavigation<CartPage, CartPageViewModel>()
                         .RegisterForNavigation<Checkout, CheckoutViewModel>()
                         .RegisterForNavigation<Detail_Item, Detail_ItemViewModel>()
                         .RegisterForNavigation<FavoritePage, FavoritePageViewModel>()
                         .RegisterForNavigation<ProductList, ProductListViewModel>()
                         .RegisterForNavigation<PassEmailEdit, PassEmailEditViewModel>()
                         .RegisterForNavigation<Account, AccountViewModel>()

                         .RegisterInstance(SemanticScreenReader.Default);

        //Services
         containerRegistry.RegisterSingleton<IVerifyInputService, VerifyInputService>()
                          .RegisterSingleton<ISettingsManager, SettingsManager>()
                          .RegisterSingleton<ISendEmail, SendEmail>()
                          .RegisterSingleton<IRepository, Repository>()
                          .RegisterSingleton<IAuth, Auth>()
                          .RegisterSingleton<IProducts, Products>()
                          .RegisterSingleton<ICart, Carts>()
                          .RegisterSingleton<IRestService, RestService>();
        //Android
#if ANDROID
        containerRegistry.RegisterSingleton<IUnfocusedEntry, Platforms.Android.Services.UnfocusedEntry>();
        containerRegistry.RegisterSingleton<IChangeThemeService, Platforms.Android.Services.ChangeThemeService>();
        containerRegistry.RegisterSingleton<ICheck_AndroidServives,
                                            Platforms.Android.Services.Permissions.Android_Services>();
        containerRegistry.RegisterSingleton<IPrintMessage, Platforms.Android.Services.PrintMessage>();

        // containerRegistry.Register<Platforms.Android.Services.Certificate.AndroidMessageHandlerEmitter>();
        //containerRegistry.Register<Platforms.Android.Services.Certificate.TrustProvider>();
        // containerRegistry.RegisterInstance(typeof(Platforms.Android.Services.Certificate.AndroidMessageHandlerEmitter));
        // containerRegistry.RegisterInstance(typeof(Platforms.Android.Services.Certificate.TrustProvider));
#endif
    }
}
