namespace NutritionTracker.View;

public partial class BarcodeScannerPage : ContentPage
{
    private bool _detected;

	public BarcodeScannerPage()
	{
		InitializeComponent();
		BarcodeScanner.Options = new ZXing.Net.Maui.BarcodeReaderOptions
		{
			Formats = ZXing.Net.Maui.BarcodeFormat.Ean13,
			Multiple = false,
			AutoRotate = true
		};
	}

    private void BarcodeScanner_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        if (_detected)      
            return;

        var first = e.Results.FirstOrDefault();

		if (first == null)
			return;

        _detected = true;

        Dispatcher.Dispatch(async () =>
        {
            var parms = new Dictionary<string, object>
            {
                { "ScannedBarcode", first.Value }
            };

            await Shell.Current.GoToAsync("..", true, parms);

            BarcodeScanner.IsDetecting = false;

        });
    }
}