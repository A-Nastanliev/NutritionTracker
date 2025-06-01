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
                foreach (var food in foods.OrderBy(f=>f.Name))
                {
                    Foods.Add(food);
                }
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
            var popup = new FoodPopup(
                foodService: foodService,
                food: food,
                onSaved: () => MainThread.BeginInvokeOnMainThread(() =>
                {
                    GetFoodsCommand.Execute(null);
                })
            );

            await Shell.Current.ShowPopupAsync(popup);
        }

    }
}
