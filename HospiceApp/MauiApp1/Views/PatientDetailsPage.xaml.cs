using MauiApp1.ViewModels;


namespace MauiApp1.Views
{
    public partial class PatientDetailsPage : ContentPage
    {
        
        public PatientDetailsPage(PatientDetailsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            
        }
        private async void OnBackButtonTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///ContactsPage"); // Navigate to the ContactsPage
        }

        private async void OnSettingsIconTapped(object sender, EventArgs e)
        {
            // Navigate to the Settings page 
            await Shell.Current.GoToAsync("///SettingsPage"); // Navigate to the SettingsPage
        }

    }
}