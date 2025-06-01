namespace NutritionTracker.ViewModels
{
    public partial class MealViewModel : BaseViewModel
    {
        [ObservableProperty]
        private Meal meal;

        public string Type => Meal.Type.ToString();

        public string MacrosDisplay =>
            $"Cals: {Meal.GetCalories():F0}, P: {Meal.GetProteins():F0}g, C: {Meal.GetCarbohydrates():F0}g, F: {Meal.GetFats():F0}g";

        public MealViewModel(Meal meal)
        {
            Meal = meal;
        }

        partial void OnMealChanged(Meal oldValue, Meal newValue)
        {
            OnPropertyChanged(nameof(Type));
            OnPropertyChanged(nameof(MacrosDisplay));
        }
    }
}
