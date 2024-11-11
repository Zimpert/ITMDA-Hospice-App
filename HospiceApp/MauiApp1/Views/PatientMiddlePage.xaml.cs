namespace MauiApp1.Views;

public partial class PatientMiddlePage : ContentPage
{
    public PatientMiddlePage()
    {
        InitializeComponent();
    }

    private async void OnViewPatientInfo(object sender, EventArgs e)
    {
        // Navigate to View Carer page 
        await Shell.Current.GoToAsync("///ContactsPage");
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

    private async void OnViewPatientMedication(object sender, EventArgs e)
    {
        // Navigate to View Carer page 
        await Shell.Current.GoToAsync("///MedicationList");
    }
}