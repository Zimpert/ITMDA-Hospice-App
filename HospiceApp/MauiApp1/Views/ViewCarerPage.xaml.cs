namespace MauiApp1.Views;

public partial class ViewCarerPage : ContentPage
{
	public ViewCarerPage()
	{
		InitializeComponent();
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