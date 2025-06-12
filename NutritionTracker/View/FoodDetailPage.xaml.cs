namespace NutritionTracker.View;

public partial class FoodDetailPage : ContentPage, IQueryAttributable
{
    public FoodDetailPage()
    {
        InitializeComponent();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var food = query.ContainsKey("Food") ? query["Food"] as Food : null;
        var onSaved = query.ContainsKey("OnSaved") ? query["OnSaved"] as Action : null;

        BindingContext = new FoodDetailViewModel(new FoodService(), onSaved, food);
    }
}