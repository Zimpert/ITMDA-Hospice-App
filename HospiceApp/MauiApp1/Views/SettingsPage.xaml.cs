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

    private async void OnSignOutTapped(object sender, EventArgs e)
    {
        SecureStorage.Remove("UserID");
        SecureStorage.Remove("Token");
        await Shell.Current.GoToAsync("///LoginPage");
    }

}