
using MauiApp1.Models;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            barcodeReader.Options = new ZXing.Net.Maui.BarcodeReaderOptions
            {
                Formats = ZXing.Net.Maui.BarcodeFormat.QrCode,
                AutoRotate = true

            };
        }


        

        private void barcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
        {
            var result = e.Results.FirstOrDefault();

            if (result == null)
            {
                return;
            }
            Dispatcher.DispatchAsync(async () =>
            {
                await DisplayAlert("Barcode Result", result.Value, "OK");
            });
        }
    }
}
