using System.Collections.ObjectModel;
using System.Diagnostics;

namespace NutritionTracker.ViewModels
{
    public partial class FoodViewModel : BaseViewModel
    {
        FoodService foodService;
        public ObservableCollection<Food> Foods { get; } = new ObservableCollection<Food>();

        [ObservableProperty]
        bool isRefreshing;

        public FoodViewModel(FoodService foodService)
        {
            Title = "Foods";
            this.foodService = foodService;
            Task.Run(() => GetFoodsAsync());
        }

        [RelayCommand]
        async Task GetFoodsAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                var foods = await foodService.ReadAllAsync();
                if (Foods.Count != 0) Foods.Clear();
                AppSettings.SortFoods(foods, Foods);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to get foods: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        private async Task DeleteFoodAsync(Food food)
        {
            bool confirm = await Shell.Current.DisplayAlert("Delete", $"Are you sure you want to delete {food.Name}?", "Yes", "No");
            if (confirm)
            {
                await foodService.DeleteAsync(food.Id);
                await GetFoodsAsync(); 
            }
        }

        [RelayCommand]
        private async Task EditFoodAsync(Food food)
        {
            await Shell.Current.GoToAsync(nameof(FoodDetailPage), true, new Dictionary<string, object>
            {
                { "Food", food },
                { "OnSaved", new Action(() => MainThread.BeginInvokeOnMainThread(() =>
                    {
                        GetFoodsCommand.Execute(null);
                    }))
                }
            });
        }

        public void EnsureSorted()
        {
            if (!AppSettings.IsSorted(Foods))
            {
                AppSettings.SortFoods(Foods);
            }
        }
    }
}
