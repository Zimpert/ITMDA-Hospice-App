using MauiApp1.ViewModels;

namespace MauiApp1.Views
{
	public partial class ShiftPage : ContentPage
	{
        public ShiftPage(CaregiverShiftViewModel csvm)
        {
            InitializeComponent();
            BindingContext = csvm;
            Task.Run(async () => await csvm.GetData());
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
}