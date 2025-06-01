namespace NutritionTracker.View;

public partial class FoodPage : ContentPage
{
    FoodViewModel foodViewModel;

    public FoodPage()
    {
        InitializeComponent();
        foodViewModel = new FoodViewModel(new FoodService());
        BindingContext = foodViewModel;
    }

    private async void OnAddFoodClicked(object sender, EventArgs e)
    {
        var popup = new FoodPopup(
            foodService: new FoodService(),
            onSaved: () => MainThread.BeginInvokeOnMainThread(() =>
            {
                foodViewModel.GetFoodsCommand.Execute(null);
            })
        );

        await this.ShowPopupAsync(popup);
    }
}