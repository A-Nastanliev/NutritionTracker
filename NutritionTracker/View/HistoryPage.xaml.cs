namespace NutritionTracker.View;

public partial class HistoryPage : ContentPage
{
    public HistoryPage(HistoryPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnMealDayCardTapped(object sender, TappedEventArgs e)
    {
        if (sender is Border border && e.Parameter is MealDayViewModel mealDayVm)
        {
            await border.ScaleTo(0.95, 80, Easing.CubicOut);
            await border.ScaleTo(1.0, 80, Easing.CubicIn);

            if (BindingContext is HistoryPageViewModel mainVm && mainVm.OpenMealDayCommand.CanExecute(mealDayVm))
            {
                mainVm.OpenMealDayCommand.Execute(mealDayVm);
            }
        }
    }
}