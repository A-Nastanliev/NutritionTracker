using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;

namespace NutritionTracker.ViewModels
{
    [QueryProperty(nameof(ScannedBarcode), "ScannedBarcode")]
    public partial class FoodDetailViewModel : BaseViewModel
    {
        private readonly FoodService foodService;
        private readonly Action onSaved;

        private string scannedBarcode;
        public string ScannedBarcode
        {
            get => scannedBarcode;
            set
            {
                SetProperty(ref scannedBarcode, value);
                _ = ApplyBarcodeAsync(value);
            }
        }

        [ObservableProperty]
        private Food food;

        [ObservableProperty]
        private bool isEditMode;

        public string ActionButtonText => IsEditMode ? "Save Changes" : "Add";

        public FoodDetailViewModel(FoodService foodService, Action onSaved, Food food = null)
        {
            this.foodService = foodService;
            this.onSaved = onSaved;
            IsEditMode = food != null;
            Food = food ?? new Food();
            Title = IsEditMode ? "Edit Food" : "Add Food";
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (Food.Calories == null) Food.Calories = 0;

            if (Food.Proteins == null) Food.Proteins = 0;

            if (Food.Carbohydrates == null) Food.Carbohydrates = 0;

            if (Food.Fats == null) Food.Fats = 0;

            if (IsEditMode)
                await foodService.UpdateAsync(Food);
            else
                await foodService.CreateAsync(Food);

            onSaved?.Invoke();

            await Shell.Current.GoToAsync("..", true);

        }

        [RelayCommand]
        private async Task ScanBarcodeAsync()
        {
            await Shell.Current.GoToAsync(nameof(BarcodeScannerPage), true);
        }

        private async Task ApplyBarcodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return;

            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("No Internet",
                    "An internet connection is required to look up nutrition info.", "OK");
                return;
            }

            try
            {
                var url = $"https://world.openfoodfacts.org/api/v0/product/{code}.json";

                using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
                using var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"HTTP {response.StatusCode}");

                var json = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("status", out var statusEl) && statusEl.GetInt32() == 1 &&
                    root.TryGetProperty("product", out var product))
                {
                    var nutriments = product.GetProperty("nutriments");

                    string name = product.GetProperty("product_name").GetString() ?? code;
                    double? kcal = nutriments.TryGetProperty("energy-kcal_100g", out var c) ? c.GetDouble() : null;
                    double? proteins = nutriments.TryGetProperty("proteins_100g", out var p) ? p.GetDouble() : null;
                    double? carbs = nutriments.TryGetProperty("carbohydrates_100g", out var cb) ? cb.GetDouble() : null;
                    double? fat = nutriments.TryGetProperty("fat_100g", out var f) ? f.GetDouble() : null;

                    Food.Name = name;
                    Food.Calories = kcal;
                    Food.Proteins = proteins;
                    Food.Carbohydrates = carbs;
                    Food.Fats = fat;

                    OnPropertyChanged(nameof(Food));
                    return;  
                }

                await Shell.Current.DisplayAlert("Not Found",
                    "Sorry, that barcode isn't in the database yet.", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Lookup Failed",
                    $"Unable to retrieve nutrition data: {ex.Message}", "OK");
            }

        }
    }
}
