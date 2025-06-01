namespace NutritionTracker
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MealDetailPage), typeof(MealDetailPage));
        }
    }
}
