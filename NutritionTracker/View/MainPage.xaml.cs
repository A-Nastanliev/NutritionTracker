namespace NutritionTracker
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is MainPageViewModel vm)
            {
                await vm.LoadTodayAsync();
            }
        }

        private async void OnMealCardTapped(object sender, TappedEventArgs e)
        {
            if (sender is Border border && e.Parameter is MealViewModel mealVm)
            {
                await border.ScaleTo(0.95, 80, Easing.CubicOut);
                await border.ScaleTo(1.0, 80, Easing.CubicIn);

                if (BindingContext is MainPageViewModel mainVm && mainVm.OpenMealCommand.CanExecute(mealVm))
                {
                    mainVm.OpenMealCommand.Execute(mealVm);
                }
            }
        }
    }
}
