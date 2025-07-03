using System.Collections.ObjectModel;

namespace NutritionTracker.ViewModels
{
    public partial class MealDetailViewModel : BaseViewModel
    {
        private readonly FoodService _foodService;
        private readonly MealFoodService _mealFoodService;

        [ObservableProperty]
        private Meal meal;
        [ObservableProperty]
        private ObservableCollection<MealFoodViewModel> mealFoods = new();
        [ObservableProperty]
        private ObservableCollection<Food> filteredFoods = new();
        [ObservableProperty]
        private string foodSearchText;
        [ObservableProperty]
        private Food selectedFood;
        [ObservableProperty]
        private string foodWeightInGrams;

        public string MealCalories => $"{Meal.GetCalories():F0} kcal";
        public string MealProteins => $"Protein: {Meal.GetProteins():F0}g";
        public string MealCarbs => $"Carbs: {Meal.GetCarbohydrates():F0}g";
        public string MealFats => $"Fats: {Meal.GetFats():F0}g";

        public MealDetailViewModel(Meal meal, DateTime date)
        {
            Meal = meal;
            _foodService = new FoodService();
            _mealFoodService = new MealFoodService();

            if (date.Date == DateTime.Today.Date) Title = $"Today's {meal.Type}";
            else Title = $"{date:MMMM dd} - {Meal.Type}";

            Meal.MealFoods ??= new List<MealFood>();
            foreach (var mf in Meal.MealFoods)
            {
                MealFoods.Add(new MealFoodViewModel(mf, ConfirmRemoveMealFoodAsync));
            }

            LoadFoodsAsync();
        }

        private async void LoadFoodsAsync()
        {
            var allFoods = await _foodService.ReadAllAsync();
            FilteredFoods = new ObservableCollection<Food>();
            AppSettings.SortFoods(allFoods, FilteredFoods);
        }

        [RelayCommand]
        private async Task AddFood()
        {
            if (SelectedFood == null || !double.TryParse(FoodWeightInGrams, out double grams) || grams <= 0)
                return;

            var mealFood = new MealFood
            {
                FoodId = SelectedFood.Id,
                Food = SelectedFood,
                Weight = grams,
                MealId = Meal.Id
            };


            await _mealFoodService.CreateAsync(mealFood);

            Meal.MealFoods.Add(mealFood);
            MealFoods.Add(new MealFoodViewModel(mealFood, ConfirmRemoveMealFoodAsync));

            OnPropertyChanged(nameof(MealCalories));
            OnPropertyChanged(nameof(MealProteins));
            OnPropertyChanged(nameof(MealFats));
            OnPropertyChanged(nameof(MealCarbs));
            FoodWeightInGrams = string.Empty;
        }

        private async Task ConfirmRemoveMealFoodAsync(MealFoodViewModel vm)
        {
            bool confirm = await Shell.Current.DisplayAlert(
                "Remove Food",
                $"Are you sure you want to remove {vm.Name}?",
                "Yes",
                "Cancel");

            if (!confirm) return;

            Meal.MealFoods.Remove(vm.MealFood);
            MealFoods.Remove(vm);
            await _mealFoodService.DeleteAsync(vm.MealFood.Id);
            OnPropertyChanged(nameof(MealCalories));
            OnPropertyChanged(nameof(MealProteins));
            OnPropertyChanged(nameof(MealFats));
            OnPropertyChanged(nameof(MealCarbs));
        }


    }
}
