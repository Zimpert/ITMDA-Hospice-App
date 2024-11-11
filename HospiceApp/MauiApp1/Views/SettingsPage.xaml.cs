namespace MauiApp1.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    private async void OnBackButtonTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///HomePage"); // Navigate to the Homepage
    }

    private async void OnSettingsIconTapped(object sender, EventArgs e)
    {
        // Navigate to the Settings page 
        await Shell.Current.GoToAsync("///SettingsPage"); // Navigate to the SettingsPage
    }

}