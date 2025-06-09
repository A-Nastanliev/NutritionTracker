using System.Collections.ObjectModel;

namespace NutritionTracker.ViewModels
{
    public partial class SettingsViewModel: BaseViewModel
    {
        private readonly FoodService foodService;
        private readonly MealFoodService mealFoodService;
        private readonly MealService mealService;
        private readonly MealDayService mealDayService;

        [ObservableProperty]
        private ObservableCollection<string> foodSortingOptions = new();
        [ObservableProperty]
        private string selectedSortingOption;

        [ObservableProperty]
        private ObservableCollection<string> accentColors = new();
        [ObservableProperty]
        private string selectedAccentColor;

        private static readonly IDictionary<string, ResourceDictionary> accentColorsMap = new Dictionary<string, ResourceDictionary>()
        {
            [NutritionTracker.AccentColors.MintGreen.ToDisplayName()] = new MintGreen(),
            [NutritionTracker.AccentColors.Lime.ToDisplayName()] = new Lime(),
            [NutritionTracker.AccentColors.Teal.ToDisplayName()] = new Teal(),
            [NutritionTracker.AccentColors.Turquoise.ToDisplayName()] = new Turquoise(),
            [NutritionTracker.AccentColors.SkyBlue.ToDisplayName()] = new SkyBlue(),
            [NutritionTracker.AccentColors.SteelBlue.ToDisplayName()] = new SteelBlue(),
            [NutritionTracker.AccentColors.Coral.ToDisplayName()] = new Coral(),
            [NutritionTracker.AccentColors.Crimson.ToDisplayName()] = new Crimson(),
            [NutritionTracker.AccentColors.Scarlet.ToDisplayName()] = new Scarlet(),
            [NutritionTracker.AccentColors.SunsetOrange.ToDisplayName()] = new SunsetOrange(),
            [NutritionTracker.AccentColors.Lavender.ToDisplayName()] = new Lavender(),
            [NutritionTracker.AccentColors.Indigo.ToDisplayName()] = new Indigo(),
            [NutritionTracker.AccentColors.Amber.ToDisplayName()] = new Amber(),
            [NutritionTracker.AccentColors.SlateGray.ToDisplayName()] = new SlateGray(),
        };

        public SettingsViewModel(FoodService foodService, MealFoodService mealFoodService, MealService mealService,MealDayService mealDayService)
        {
            Title = "Settings";
            this.mealDayService = mealDayService;
            this.mealService = mealService;
            this.mealFoodService = mealFoodService;
            this.foodService = foodService;

            foodSortingOptions = new ObservableCollection<string>(EnumDisplayHelper.foodSortNames.Values);
            SelectedSortingOption = AppSettings.CurrentFoodSortDisplayName;

            accentColors = new ObservableCollection<string>(EnumDisplayHelper.accentColorsNames.Values);
            SelectedAccentColor = AppSettings.CurrentColorDisplayName;

        }

        [RelayCommand]
        private async Task ClearDatabaseAsync()
        {
            bool confirm = await Shell.Current.DisplayAlert("Delete database", "Are you sure? This cannot be undone."
                , "Yes", "No");
            if (confirm)
            {
                await foodService.ClearAsync();
                await mealFoodService.ClearAsync();
                await mealService.ClearAsync();
                await mealDayService.ClearAsync();
            }
        }

        [RelayCommand]
        private async Task DeleteMealHistoryAsync()
        {
            bool confirm = await Shell.Current.DisplayAlert("Delete meal history", "Are you sure? This cannot be undone."
                , "Yes", "No");
            if (confirm)
            {
                await mealFoodService.ClearAsync();
                await mealService.ClearAsync();
                await mealDayService.ClearAsync();
            }
        }

        partial void OnSelectedSortingOptionChanged(string value)
        {
            var entry = EnumDisplayHelper.foodSortNames
                .FirstOrDefault(x => x.Value == value);

            if (!entry.Equals(default))
            {
                AppSettings.CurrentFoodSortOption = entry.Key;
            }
        }

        partial void OnSelectedAccentColorChanged(string value)
        {
            var entry = EnumDisplayHelper.accentColorsNames
               .FirstOrDefault(x => x.Value == value);

            if (!entry.Equals(default))
            {
                SetAccentColor(value);
                AppSettings.CurrentColor = entry.Key;
            }
        }

        public static void SetAccentColor(string accentColorName, bool firstTime = false)
        {
            if (AppSettings.CurrentColor.ToDisplayName() == accentColorName && !firstTime)
                return;

            var colorToBeApplied = accentColorsMap[accentColorName];
            var colorToBeRemoved = accentColorsMap[AppSettings.CurrentColor.ToDisplayName()];
        
            Application.Current.Resources.MergedDictionaries.Remove(colorToBeRemoved);
            Application.Current.Resources.MergedDictionaries.Add(colorToBeApplied);
        }
    }
}
