namespace NutritionTracker.ViewModels
{
    public partial class MealDayViewModel : BaseViewModel
    {
        public MealDay MealDay { get; }

        public MealDayViewModel(MealDay mealDay)
        {
            MealDay = mealDay;
        }

        public string DateText => MealDay.Date.ToString("MMMM dd, yyyy");

        public string CaloriesText => $"Total Calories: {MealDay.GetCalories():F0} kcal";

        public string MacrosText =>
            $"P: {MealDay.GetProteins():F0}g, C: {MealDay.GetCarbohydrates():F0}g, F: {MealDay.GetFats():F0}g";

        public string MealCountText => $"Meals: {MealDay.Meals?.Count ?? 0}";

        public Command DeleteMealDayCommand { get; set; }
    }
}
