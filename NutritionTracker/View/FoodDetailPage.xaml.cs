namespace NutritionTracker.View;

public partial class FoodDetailPage : ContentPage, IQueryAttributable
{
 
    public FoodDetailPage()
    {
        InitializeComponent();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (BindingContext is not FoodDetailViewModel vm)
        {
            var food = query.TryGetValue("Food", out var f) ? f as Food : null;
            var onSaved = query.TryGetValue("OnSaved", out var a) ? a as Action : null;

            vm = new FoodDetailViewModel(new FoodService(), onSaved, food);
            BindingContext = vm;
        }

        if (query.TryGetValue("ScannedBarcode", out var b) && b is string code)
            ((FoodDetailViewModel)BindingContext).ScannedBarcode = code;
    }
}