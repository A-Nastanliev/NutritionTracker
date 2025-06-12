namespace NutritionTracker.ViewModels
{
    public partial class FoodDetailViewModel : BaseViewModel
    {
        private readonly FoodService foodService;
        private readonly Action onSaved;

        [ObservableProperty]
        private Food food;

        [ObservableProperty]
        private bool isEditMode;

        public string ActionButtonText => IsEditMode ? "Save Changes" : "Add";

        public FoodDetailViewModel(FoodService foodService, Action onSaved, Food food = null)
        {
            this.foodService = foodService;
            this.onSaved = onSaved;
            IsEditMode = food != null;
            Food = food ?? new Food();
            Title = IsEditMode ? "Edit Food" : "Add Food";
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (Food.Calories == null) Food.Calories = 0;

            if (Food.Proteins == null) Food.Proteins = 0;

            if (Food.Carbohydrates == null) Food.Carbohydrates = 0;

            if (Food.Fats == null) Food.Fats = 0;

            if (IsEditMode)
                await foodService.UpdateAsync(Food);
            else
                await foodService.CreateAsync(Food);

            onSaved?.Invoke();

            await Shell.Current.GoToAsync("..");

        }
    }
}
