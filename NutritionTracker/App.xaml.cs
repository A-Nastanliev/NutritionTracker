namespace NutritionTracker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            SettingsViewModel.SetAccentColor(AppSettings.CurrentColor.ToDisplayName(), true);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}