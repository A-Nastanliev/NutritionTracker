namespace NutritionTracker
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MealDetailPage), typeof(MealDetailPage));
            Routing.RegisterRoute(nameof(MealDayPage), typeof(MealDayPage));
            Routing.RegisterRoute(nameof(FoodDetailPage), typeof(FoodDetailPage));
        }
    }
}
