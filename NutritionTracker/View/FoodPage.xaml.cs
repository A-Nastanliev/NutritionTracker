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

        await Shell.Current.GoToAsync(nameof(FoodDetailPage), true, new Dictionary<string, object>
        {
            { "Food", null },
            { "OnSaved", new Action(() => MainThread.BeginInvokeOnMainThread(() =>
                {
                    foodViewModel.GetFoodsCommand.Execute(null);
                }))
            }
        });
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is FoodViewModel vm)
        {
            vm.EnsureSorted();
        }
    }
}