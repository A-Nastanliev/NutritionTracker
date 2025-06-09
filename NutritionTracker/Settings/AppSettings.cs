using System.Collections.ObjectModel;

namespace NutritionTracker
{
    public static class AppSettings
    {
        private const string SortOptionKey = "FoodSortingOption";
        private const string AccentColorKey = "AccentColor";

        private static FoodSortOption? currentFoodSortOption;
        private static AccentColors? currentColor;

        public static FoodSortOption CurrentFoodSortOption
        {
            get
            {
                if (currentFoodSortOption == null)
                {
                    var saved = Preferences.Default.Get(SortOptionKey, (int)FoodSortOption.AlphabeticalAsc);
                    currentFoodSortOption = (FoodSortOption)saved;
                }

                return currentFoodSortOption.Value;
            }

            set
            {
                currentFoodSortOption = value;
                Preferences.Default.Set(SortOptionKey, (int)value);
            }
        }

        public static AccentColors CurrentColor
        {
            get
            {
                if (currentColor == null)
                {
                    var saved = Preferences.Default.Get(AccentColorKey, (int)AccentColors.MintGreen);
                    currentColor = (AccentColors)saved;
                }

                return currentColor.Value;
            }

            set
            {
                currentColor = value;
                Preferences.Default.Set(AccentColorKey, (int)value);
            }
        }

        public static string CurrentFoodSortDisplayName => CurrentFoodSortOption.ToDisplayName();
        public static string CurrentColorDisplayName => CurrentColor.ToDisplayName();

        public static void SortFoods(List<Food> unsortedFoods, ObservableCollection<Food> targetCollection)
        {
            var sorted = GetSortedFoods(unsortedFoods);
            foreach (var food in sorted)
                targetCollection.Add(food);
        }

        public static void SortFoods(ObservableCollection<Food> collection)
        {
            var sorted = GetSortedFoods(collection).ToList();

            for (int i = 0; i < sorted.Count; i++)
            {
                var item = sorted[i];
                var currentIndex = collection.IndexOf(item);
                if (currentIndex != i)
                {
                    collection.Move(currentIndex, i);
                }
            }
        }

        private static IEnumerable<Food> GetSortedFoods(IEnumerable<Food> foods)
        {
            Func<Food, object> keySelector = CurrentFoodSortOption switch
            {
                FoodSortOption.AlphabeticalAsc or FoodSortOption.AlphabeticalDesc => f => f.Name,
                FoodSortOption.HighestProtein or FoodSortOption.LowestProtein => f => f.Proteins,
                FoodSortOption.HighestCarbs or FoodSortOption.LowestCarbs => f => f.Carbohydrates,
                FoodSortOption.HighestFats or FoodSortOption.LowestFats => f => f.Fats,
                FoodSortOption.HighestCalories or FoodSortOption.LowestCalories => f => f.Calories,
                _ => f => f.Name
            };

            bool descending = CurrentFoodSortOption switch
            {
                FoodSortOption.AlphabeticalDesc or
                FoodSortOption.HighestProtein or
                FoodSortOption.HighestCarbs or
                FoodSortOption.HighestFats or
                FoodSortOption.HighestCalories => true,
                _ => false
            };

            return descending
                ? foods.OrderByDescending(keySelector)
                : foods.OrderBy(keySelector);
        }

        public static bool IsSorted(IList<Food> foods)
        {
            if (foods.Count < 2) return true;

            Func<Food, object> keySelector = CurrentFoodSortOption switch
            {
                FoodSortOption.AlphabeticalAsc or FoodSortOption.AlphabeticalDesc => f => f.Name,
                FoodSortOption.HighestProtein or FoodSortOption.LowestProtein => f => f.Proteins,
                FoodSortOption.HighestCarbs or FoodSortOption.LowestCarbs => f => f.Carbohydrates,
                FoodSortOption.HighestFats or FoodSortOption.LowestFats => f => f.Fats,
                FoodSortOption.HighestCalories or FoodSortOption.LowestCalories => f => f.Calories,
                _ => f => f.Name
            };

            bool descending = CurrentFoodSortOption switch
            {
                FoodSortOption.AlphabeticalDesc or
                FoodSortOption.HighestProtein or
                FoodSortOption.HighestCarbs or
                FoodSortOption.HighestFats or
                FoodSortOption.HighestCalories => true,
                _ => false
            };

            var comparer = Comparer<object>.Default;

            for (int i = 1; i < foods.Count; i++)
            {
                var prev = keySelector(foods[i - 1]);
                var current = keySelector(foods[i]);
                int comparison = comparer.Compare(prev, current);

                if ((descending && comparison < 0) || (!descending && comparison > 0))
                    return false;
            }

            return true;
        }


    }
}
