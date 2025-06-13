using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace NutritionTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseBarcodeReader();

            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<FoodService>();
            builder.Services.AddSingleton<MealFoodService>();
            builder.Services.AddSingleton<MealService>();
            builder.Services.AddSingleton<MealDayService>();

            builder.Services.AddTransient<FoodDetailPage>();
            builder.Services.AddTransient<MealDetailPage>();
            builder.Services.AddTransient<MealDayPage>();
            builder.Services.AddTransient<BarcodeScannerPage>();

            builder.Services.AddSingleton<FoodPage>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<HistoryPage>();
            builder.Services.AddSingleton<SettingsPage>();

            builder.Services.AddSingleton<FoodViewModel>();
            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddSingleton<HistoryPageViewModel>();
            builder.Services.AddSingleton<SettingsViewModel>();

            builder.Services.AddTransient<FoodDetailPage>();
            builder.Services.AddTransient<MealFoodViewModel>();
            builder.Services.AddTransient<MealDetailViewModel>();
            builder.Services.AddTransient<MealViewModel>();
            builder.Services.AddTransient<MealDayViewModel>();
            builder.Services.AddTransient<MealDayPageViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            Task.Run(() => DatabaseService.InitAsync());

            return builder.Build();
        }
    }
}
