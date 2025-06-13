using System.Collections.ObjectModel;

namespace NutritionTracker.ViewModels
{
    public partial class MealDayPageViewModel : BaseViewModel
    {
        private readonly MealDayService _mealDayService = new();
        private readonly MealService _mealService = new();
        private readonly MealDay _mealDay;

        public ObservableCollection<MealViewModel> Meals { get; } = new();

        [ObservableProperty] private string totalCalories;
        [ObservableProperty] private string totalProteins;
        [ObservableProperty] private string totalFats;
        [ObservableProperty] private string totalCarbs;

        public MealDayPageViewModel(MealDay mealDay)
        {
            Title = mealDay.Date == DateTime.Today ? "Today's Meals" : mealDay.Date.ToString("MMMM dd");
            _mealDay = mealDay;

            _mealDay.Meals ??= new List<Meal>();
            foreach (var meal in _mealDay.Meals)
            {
                Meals.Add(new MealViewModel(meal));
            }

            UpdateSummary();
        }

        private void UpdateSummary()
        {
            TotalCalories = $"Calories: {_mealDay.GetCalories():F0}";
            TotalProteins = $"Proteins: {_mealDay.GetProteins():F0}g";
            TotalFats = $"Fats: {_mealDay.GetFats():F0}g";
            TotalCarbs = $"Carbs: {_mealDay.GetCarbohydrates():F0}g";
        }

        [RelayCommand]
        private async Task OpenMealAsync(MealViewModel mealViewModel)
        {
            if (mealViewModel is null)
                return;

            await Shell.Current.GoToAsync(nameof(MealDetailPage),true,  new Dictionary<string, object>
            {
                ["Meal"] = mealViewModel.Meal,
                 ["MealDate"] = _mealDay.Date
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
                MealDayId = _mealDay.Id
            };

            await _mealService.CreateAsync(newMeal);

            _mealDay.Meals.Add(newMeal);
            await _mealDayService.UpdateAsync(_mealDay);

            Meals.Add(new MealViewModel(newMeal));
            UpdateSummary();

            await Shell.Current.GoToAsync(nameof(MealDetailPage),true,  new Dictionary<string, object>
            {
                ["Meal"] = newMeal,
                ["MealDate"] = _mealDay.Date
            });
  
        }

        [RelayCommand]
        private async Task DeleteMealAsync(MealViewModel mealVm)
        {
            if (mealVm == null)
                return;

            bool confirm = await Shell.Current.DisplayAlert(
                "Delete Meal", $"Are you sure you want to delete this {mealVm.Meal.Type}?", "Yes", "Cancel");

            if (!confirm)
                return;

            await _mealService.DeleteAsync(mealVm.Meal.Id);

            _mealDay.Meals.Remove(mealVm.Meal);
            await _mealDayService.UpdateAsync(_mealDay);

            Meals.Remove(mealVm);
            UpdateSummary();
        }
    }

}
