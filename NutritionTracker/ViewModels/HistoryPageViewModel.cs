using System.Collections.ObjectModel;
using System.Diagnostics;

namespace NutritionTracker.ViewModels
{
    public partial class HistoryPageViewModel : BaseViewModel
    {
        private readonly MealDayService mealDayService;

        public ObservableCollection<MealDayViewModel> MealDays { get; } = new();

        [ObservableProperty]
        private bool isRefreshing;

        public HistoryPageViewModel(MealDayService mealDayService)
        {
            Title = "History";
            this.mealDayService = mealDayService;
            Task.Run(() => GetMealDaysAsync());
        }

        [RelayCommand]
        private async Task GetMealDaysAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                var days = await mealDayService.ReadAllAsync();
                days = days.Where(d => d.Date.Date != DateTime.Today).Reverse().ToList();

                MainThread.BeginInvokeOnMainThread(() => MealDays.Clear());

                foreach (var day in days)
                {
                    var vm = new MealDayViewModel(day);
                    vm.DeleteMealDayCommand = new Command(async () => await DeleteMealDayAsync(day));
                    MainThread.BeginInvokeOnMainThread(() => MealDays.Add(vm));
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        private async Task DeleteMealDayAsync(MealDay mealDay)
        {
            bool confirm = await Shell.Current.DisplayAlert("Delete", $"Are you sure you want to delete {mealDay.Date:MMMM dd}?", "Yes", "No");
            if (confirm)
            {
                await mealDayService.DeleteAsync(mealDay.Id);
                await GetMealDaysAsync();
            }
        }

        [RelayCommand]
        private async Task OpenMealDayAsync(MealDayViewModel vm)
        {
            if (vm == null)
                return;

           await Shell.Current.GoToAsync(nameof(MealDayPage), new Dictionary<string, object>
            {
                ["MealDay"] = vm.MealDay
            });
        
        }
    }
}
