namespace NutritionTracker.ViewModels
{
    public partial class MealDayViewModel : BaseViewModel
    {
        public MealDay MealDay { get; }

        public MealDayViewModel(MealDay mealDay)
        {
            MealDay = mealDay;
        }

        public string DateText => MealDay.Date.ToString("dd/MM/yy");
        public string CaloriesText => $"{MealDay.GetCalories():F0} kcal";
        public string ProteinText => $"{MealDay.GetProteins():F0}g";
        public string CarbsText => $"{MealDay.GetCarbohydrates():F0}g";
        public string FatText => $"{MealDay.GetFats():F0}g";

        public string MealCountText => $"Meals: {MealDay.Meals?.Count ?? 0}";

        public Command DeleteMealDayCommand { get; set; }
    }
}
