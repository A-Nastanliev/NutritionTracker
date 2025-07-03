namespace NutritionTracker.ViewModels
{
    public partial class MealFoodViewModel : BaseViewModel
    {
        public MealFood MealFood { get; }
        public string Name => MealFood.Food?.Name ?? "Unknown";

        public string WeightText => $"{MealFood.Weight}g";
        public string CaloriesText => $"{MealFood.GetCalories():F0} kcal";
        public string ProteinText => $"Proteins: {MealFood.GetProteins():F0}g";
        public string CarbText => $"Carbs: {MealFood.GetCarbohydrates():F0}g";
        public string FatText => $"Fats: {MealFood.GetFats():F0}g";

        private readonly Func<MealFoodViewModel, Task> _onRemoveAsync;

        public MealFoodViewModel(MealFood mealFood, Func<MealFoodViewModel, Task> onRemoveAsync)
        {
            MealFood = mealFood;
            _onRemoveAsync = onRemoveAsync;
        }

        [RelayCommand]
        private async Task Remove()
        {
            if (_onRemoveAsync != null)
                await _onRemoveAsync(this);
        }
    }
}
