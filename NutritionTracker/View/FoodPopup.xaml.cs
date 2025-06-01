namespace NutritionTracker.View;

public partial class FoodPopup : Popup
{
    public FoodPopup(FoodService foodService, Action onSaved, Food food = null)
    {
        InitializeComponent();
        var viewModel = new FoodPopupViewModel(foodService, onSaved, this, food);
        BindingContext = viewModel;
        var screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;

        Size = new Size(screenWidth * 0.8, -1);

    }
}