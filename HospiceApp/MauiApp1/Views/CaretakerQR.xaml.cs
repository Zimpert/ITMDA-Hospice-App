
using MauiApp1.ViewModels;

namespace MauiApp1.Views;

public partial class CaretakerQR : ContentPage
{
	public CaretakerQR()
	{
		InitializeComponent();
		BindingContext = new QRGenerationViewModel();
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