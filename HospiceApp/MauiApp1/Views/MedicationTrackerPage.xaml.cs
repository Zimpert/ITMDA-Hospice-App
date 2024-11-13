using MauiApp1.ViewModels;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace MauiApp1.Views
{
    public partial class MedicationTrackerPage : ContentPage
    {
        public MedicationTrackerPage(MedicationTrackerViewModel mvm)
        {
            InitializeComponent();

            // Sample data for testing

            BindingContext = mvm;
        }

        private async void OnSettingsIconTapped(object sender, EventArgs e)
        {
            // Navigate to the Settings page 
            await Shell.Current.GoToAsync("///SettingsPage"); // Navigate to the SettingsPage
        }

        private async void OnBackButtonTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///ContactsPage"); // Navigate back to the previous page
        }
    }
}
