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
                });

            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<FoodService>();
            builder.Services.AddSingleton<MealFoodService>();
            builder.Services.AddSingleton<MealService>();
            builder.Services.AddSingleton<MealDayService>();

            builder.Services.AddTransient<FoodPopup>();
            builder.Services.AddTransient<MealDetailPage>();

            builder.Services.AddSingleton<FoodPage>();
            builder.Services.AddSingleton<MainPage>();

            builder.Services.AddSingleton<FoodViewModel>();
            builder.Services.AddSingleton<MainPageViewModel>();

            builder.Services.AddTransient<FoodPopupViewModel>();
            builder.Services.AddTransient<MealDetailViewModel>();
            builder.Services.AddTransient<MealViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            Task.Run(() => DatabaseService.InitAsync());

            return builder.Build();
        }
    }
}
