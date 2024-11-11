
using MauiApp1.ViewModels;

namespace MauiApp1.Views;

public partial class CaretakerQR : ContentPage
{
	public CaretakerQR()
	{
		InitializeComponent();
		BindingContext = new QRGenerationViewModel();
	}

    private void OnSaveButtonClicked(object sender, EventArgs e)
    {
        // Your code to handle the save action goes here.
        // For now, you can display a simple alert to confirm the button click.
        DisplayAlert("Save", "Save to Camera Roll button clicked.", "OK");
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