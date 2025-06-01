namespace NutritionTracker.View;

[QueryProperty(nameof(Meal), "Meal")]
[QueryProperty(nameof(MealDate), "MealDate")]
public partial class MealDetailPage : ContentPage
{
    public MealDetailPage()
    {
        InitializeComponent();
    }

    private Meal _meal;
    private DateTime _mealDate;

    public Meal Meal
    {
        get => _meal;
        set
        {
            _meal = value;
            SetBindingContextIfReady();
        }
    }

    public DateTime MealDate
    {
        get => _mealDate;
        set
        {
            _mealDate = value;
            SetBindingContextIfReady();
        }
    }

    private void SetBindingContextIfReady()
    {
        if (_meal != null && _mealDate != default)
        {
            BindingContext = new MealDetailViewModel(_meal, _mealDate);
        }
    }

}