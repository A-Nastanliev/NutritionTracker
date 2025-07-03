namespace NutritionTracker.ViewModels
{
    public partial class MealViewModel : BaseViewModel
    {
        [ObservableProperty]
        private Meal meal;

        public string Type => Meal.Type.ToString();

        public string CalorieText => $"{Meal.GetCalories():F0} kcal";

        public string ProteinText => $"P: {Meal.GetProteins():F0}g";

        public string CarbohydrateText => $"C: {Meal.GetCarbohydrates():F0}g";

        public string FatText => $"F: {Meal.GetFats():F0}g";

        public MealViewModel(Meal meal)
        {
            Meal = meal;
        }

        partial void OnMealChanged(Meal oldValue, Meal newValue)
        {
            OnPropertyChanged(nameof(Type));
            OnPropertyChanged(nameof(CalorieText));
            OnPropertyChanged(nameof(ProteinText));
            OnPropertyChanged(nameof(CarbohydrateText));
            OnPropertyChanged(nameof(FatText));
        }
    }
}
