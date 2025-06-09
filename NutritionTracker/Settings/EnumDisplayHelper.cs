namespace NutritionTracker
{
    public static class EnumDisplayHelper
    {
        public static readonly Dictionary<FoodSortOption, string> foodSortNames = new()
        {
            [FoodSortOption.AlphabeticalAsc] = "A → Z",
            [FoodSortOption.AlphabeticalDesc] = "Z → A",
            [FoodSortOption.HighestProtein] = "Highest Protein",
            [FoodSortOption.LowestProtein] = "Lowest Protein",
            [FoodSortOption.HighestCarbs] = "Highest Carbs",
            [FoodSortOption.LowestCarbs] = "Lowest Carbs",
            [FoodSortOption.HighestFats] = "Highest Fats",
            [FoodSortOption.LowestFats] = "Lowest Fats",
            [FoodSortOption.HighestCalories] = "Highest Calories",
            [FoodSortOption.LowestCalories] = "Lowest Calories",
        };

        public static readonly Dictionary<AccentColors, string> accentColorsNames = new() 
        {
            [AccentColors.MintGreen] = "Mint Green",
            [AccentColors.Lime] = "Lime",
            [AccentColors.Teal] = "Teal",
            [AccentColors.Turquoise] = "Turquoise",
            [AccentColors.SkyBlue] = "Sky Blue",
            [AccentColors.SteelBlue] = "Steel Blue",
            [AccentColors.Coral] = "Coral",
            [AccentColors.Crimson] = "Crimson",
            [AccentColors.Scarlet] = "Scarlet",
            [AccentColors.SunsetOrange] = "Sunset Orange",
            [AccentColors.Lavender] = "Lavender",
            [AccentColors.Indigo] = "Indigo",
            [AccentColors.Amber] = "Amber",
            [AccentColors.SlateGray] = "Slate Gray",
        };

        public static string ToDisplayName(this FoodSortOption option) =>
           foodSortNames.TryGetValue(option, out var name) ? name : option.ToString();

        public static string ToDisplayName(this AccentColors color) =>
            accentColorsNames.TryGetValue(color, out var name) ? name : color.ToString();

    }
}
