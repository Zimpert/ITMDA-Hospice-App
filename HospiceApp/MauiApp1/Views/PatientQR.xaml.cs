
using MauiApp1.Interfaces;
using ZXing.Net.Maui;

namespace MauiApp1.Views;

public partial class PatientQR : ContentPage
{
    private readonly IRequestManager _reqMan;
	public PatientQR(IRequestManager regMan)
	{
		InitializeComponent();
        ConfigureBarcodeReader();
        _reqMan = regMan;
	}

    private void ConfigureBarcodeReader()
    {
        cameraBarcodeReaderView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.TwoDimensional,
            AutoRotate = true,
            Multiple = false
        };
    }

    protected async void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        // here if it's detected we need to call request manager and just log the value 
        // simple!
        
        var first = e.Results?.FirstOrDefault();
        if (first is null)
        {
            return;
        }
        cameraBarcodeReaderView.IsDetecting = false;
        var token = await SecureStorage.GetAsync("Token");
        await _reqMan.ShiftLog(token, first.Value);
        Dispatcher.DispatchAsync(async () =>
        {
            await Shell.Current.GoToAsync("///HomePage");
        });

            
    }

}