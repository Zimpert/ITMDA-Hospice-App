
using MauiApp1.ViewModels;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace MauiApp1.Views;

public partial class CaretakerQR : ContentPage
{
	public CaretakerQR(QRGenerationViewModel qrVM)
	{
		InitializeComponent();
        BindingContext = qrVM;

    }


    private async void OnBackButtonTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///HomePage"); // Navigate to the Homepage
    }

    private async void OnSettingsIconTapped(object sender, EventArgs e)
    {
     
        await Shell.Current.GoToAsync("///SettingsPage"); // Navigate to the SettingsPage
    }

}