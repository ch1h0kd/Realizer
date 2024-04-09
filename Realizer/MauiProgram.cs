using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Maui;
using Realizer.Data;
using Realizer.ViewModels;
using System.Net.Mail;
using Realizer.Pages;
using Realizer.Services;
using Syncfusion.Maui.Core.Hosting;
using Realizer.Resources.SearchHandlers;

namespace Realizer
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.ConfigureSyncfusionCore();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseMauiCommunityToolkit();
#if true

#endif

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<DatabaseContext>();

            //builder.Services.AddSingleton<INavigationService, NavigationService>();

            //UI registration
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<ClientsPage>();
            builder.Services.AddTransient<ProductsPage>();
            builder.Services.AddTransient<NewClientPage>();
            builder.Services.AddTransient<NewProductPage>();
            builder.Services.AddTransient<ClientIndivPage>();
            builder.Services.AddSingleton<ProductIndivPage>();
            builder.Services.AddTransient<NewVisitPage>();
            builder.Services.AddTransient<HistoryPage>();
            builder.Services.AddTransient<NewVisitPage>();
            builder.Services.AddTransient<ClientEditPage>();

            //viewModel registration
            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddTransient<ClientsViewModel>();
            builder.Services.AddTransient<ProductsViewModel>();
            builder.Services.AddTransient<PhoneNumViewModel>();
            builder.Services.AddTransient<ProductIndivViewModel>();
            builder.Services.AddTransient<HistoryViewModel>();
            builder.Services.AddTransient<PurchaseViewModel>();
            builder.Services.AddTransient<ProductSearchHandler>();
            return builder.Build();
        }
    }
}
