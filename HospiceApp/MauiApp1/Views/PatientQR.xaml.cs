
using ZXing.Net.Maui;

namespace MauiApp1.Views;

public partial class PatientQR : ContentPage
{
	public PatientQR()
	{
		InitializeComponent();
        ConfigureBarcodeReader();
	}

    private void ConfigureBarcodeReader()
    {
        cameraBarcodeReaderView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.TwoDimensional,
            AutoRotate = true,
            Multiple = true
        };
    }

    protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        foreach (var barcode in e.Results)
            Console.WriteLine($"Barcodes: {barcode.Format} -> {barcode.Value}");
    }

}