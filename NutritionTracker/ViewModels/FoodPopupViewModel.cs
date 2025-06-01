namespace NutritionTracker.ViewModels
{
    public partial class FoodPopupViewModel : BaseViewModel
    {
        private readonly FoodService foodService;
        private readonly Action onSaved;
        private readonly Popup popup;

        [ObservableProperty]
        private Food food;

        [ObservableProperty]
        private bool isEditMode;

        public string Header => IsEditMode ? "Edit Food" : "Add Food";
        public string ActionButtonText => IsEditMode ? "Save Changes" : "Add";

        public FoodPopupViewModel(FoodService foodService, Action onSaved, Popup popup, Food food = null)
        {
            this.foodService = foodService;
            this.onSaved = onSaved;
            this.popup = popup;
            IsEditMode = food != null;
            Food = food ?? new Food(); 
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (Food.Calories == null) Food.Calories = 0;

            if(Food.Proteins == null) Food.Proteins = 0;

            if (Food.Carbohydrates == null) Food.Carbohydrates = 0;

            if(Food.Fats == null) Food.Fats = 0;

            if (IsEditMode)
                await foodService.UpdateAsync(Food);
            else
                await foodService.CreateAsync(Food);

            onSaved?.Invoke();

            popup.Close();

        }
    }
}
