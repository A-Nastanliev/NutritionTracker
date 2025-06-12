using System.Collections.ObjectModel;

namespace NutritionTracker.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        private readonly MealDayService _mealDayService;
        private readonly MealService _mealService;

        [ObservableProperty]
        private MealDay todayMealDay;

        [ObservableProperty]
        private ObservableCollection<MealViewModel> meals = new();

        [ObservableProperty]
        private string totalCalories;

        [ObservableProperty]
        private string totalProteins;

        [ObservableProperty]
        private string totalFats;

        [ObservableProperty]
        private string totalCarbs;

        public MainPageViewModel()
        {
            Title = "Today's Meals";
            _mealDayService = new MealDayService();
            _mealService = new MealService();
            LoadTodayAsync();
        }

        public async Task LoadTodayAsync()
        {
            var all = await _mealDayService.ReadAllAsync();
            var todays = all.Where(m => m.Date.Date == DateTime.Today.Date).ToList();

            if (todays.Count > 1)
            {
                for (int i = 1; i < todays.Count; i++)
                {
                    await _mealDayService.DeleteAsync(todays[i].Id);
                }
            }

            TodayMealDay = todays.FirstOrDefault();

            if (TodayMealDay == null)
            {
                TodayMealDay = new MealDay { Date = DateTime.Today, Meals = new List<Meal>() };
                await _mealDayService.CreateAsync(TodayMealDay);
            }

            TodayMealDay.Meals ??= new List<Meal>();
            Meals.Clear();
            foreach (var meal in TodayMealDay.Meals)
            {
                Meals.Add(new MealViewModel(meal));
            }

            UpdateSummary();
        }

        private void UpdateSummary()
        {
            TotalCalories = $"Calories: {TodayMealDay.GetCalories():F0}";
            TotalProteins = $"Proteins: {TodayMealDay.GetProteins():F0}g";
            TotalFats = $"Fats: {TodayMealDay.GetFats():F0}g";
            TotalCarbs = $"Carbs: {TodayMealDay.GetCarbohydrates():F0}g";
        }

        [RelayCommand]
        private async Task OpenMealAsync(MealViewModel mealViewModel)
        {
            if (mealViewModel is null)
                return;

            await Shell.Current.GoToAsync(nameof(MealDetailPage), new Dictionary<string, object>
            {
                ["Meal"] = mealViewModel.Meal,
                ["MealDate"] = TodayMealDay.Date
            });
        }

        [RelayCommand]
        private async Task AddMealAsync()
        {
            string action = await Shell.Current.DisplayActionSheet(
                "Select Meal Type", "Cancel", null,
                Enum.GetNames(typeof(MealType)));

            if (string.IsNullOrEmpty(action) || action == "Cancel")
                return;

            if (!Enum.TryParse<MealType>(action, out var selectedType))
                return;

            var newMeal = new Meal
            {
                Type = selectedType,
                MealFoods = new List<MealFood>(),
                MealDayId = TodayMealDay.Id
            };

            await _mealService.CreateAsync(newMeal);

            TodayMealDay.Meals.Add(newMeal);
            await _mealDayService.UpdateAsync(TodayMealDay);

            var mealVm = new MealViewModel(newMeal);
            Meals.Add(mealVm);
            UpdateSummary();

            await Shell.Current.GoToAsync(nameof(MealDetailPage), new Dictionary<string, object>
            {
                ["Meal"] = newMeal,
                ["MealDate"] = TodayMealDay.Date
            });

        }

        [RelayCommand]
        private async Task DeleteMealAsync(MealViewModel mealVm)
        {
            if (mealVm == null)
                return;

            bool confirm = await Shell.Current.DisplayAlert(
                "Delete Meal", "Are you sure you want to delete this meal?", "Yes", "Cancel");

            if (!confirm)
                return;

            await _mealService.DeleteAsync(mealVm.Meal.Id);

            TodayMealDay.Meals.Remove(mealVm.Meal);
            await _mealDayService.UpdateAsync(TodayMealDay);


            Meals.Remove(mealVm);
            UpdateSummary();
        }
    }
}
