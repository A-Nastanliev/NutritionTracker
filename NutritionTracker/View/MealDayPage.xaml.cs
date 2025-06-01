namespace NutritionTracker.View;

[QueryProperty(nameof(MealDay), "MealDay")]
public partial class MealDayPage : ContentPage
{
    public MealDay MealDay
    {
        set => BindingContext = new MealDayPageViewModel(value);
    }

    public MealDayPage()
    {
        InitializeComponent();
    }

    private async void OnMealCardTapped(object sender, TappedEventArgs e)
    {
        if (sender is Border border && e.Parameter is MealViewModel mealVm)
        {
            await border.ScaleTo(0.95, 80, Easing.CubicOut);
            await border.ScaleTo(1.0, 80, Easing.CubicIn);

            if (BindingContext is MealDayPageViewModel mainVm && mainVm.OpenMealCommand.CanExecute(mealVm))
            {
                mainVm.OpenMealCommand.Execute(mealVm);
            }
        }
    }


}