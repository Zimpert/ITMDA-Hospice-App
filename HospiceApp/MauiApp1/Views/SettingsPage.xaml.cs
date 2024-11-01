using Microsoft.Maui.Controls;
using MauiApp1.Styles; // Ensure this matches your project structure

namespace MauiApp1
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            // Set the initial state of the switch based on the current theme
            ThemeSwitch.IsToggled = Application.Current.UserAppTheme == AppTheme.Dark;
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

        private void OnThemeSwitchToggled(object sender, ToggledEventArgs e)
        {
            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;

            // Remove the current theme dictionary
            var existingTheme = mergedDictionaries
                .FirstOrDefault(d => d is LightTheme || d is DarkTheme);

            if (existingTheme != null)
            {
                mergedDictionaries.Remove(existingTheme);
            }

            // Apply the new theme
            if (e.Value)
            {
                // Apply Dark Theme
                mergedDictionaries.Add(new DarkTheme());
                Application.Current.UserAppTheme = AppTheme.Dark;
            }
            else
            {
                // Apply Light Theme
                mergedDictionaries.Add(new LightTheme());
                Application.Current.UserAppTheme = AppTheme.Light;
            }

            // Save the theme preference if needed
            Preferences.Set("UserAppTheme", e.Value ? "Dark" : "Light");
        }
    }
}


