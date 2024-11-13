namespace MauiApp1.Views;
using Microsoft.Maui.Controls;
using MauiApp1.Services;
using System;
using System.Windows.Input;
using MauiApp1.ViewModels;

public partial class HomePage : ContentPage
{

    public HomePage(HomePageViewModel hpvm)
    {
        InitializeComponent();
        BindingContext = hpvm;

    }
    // Event handlers 
    private async void OnViewCarerClicked(object sender, EventArgs e)
    {
        // Navigate to View Carer page 
        await Shell.Current.GoToAsync("///ContactsPage");
    }

    private async void OnNotificationPageClicked(object sender, EventArgs e)
    {
        // Navigate to Notification Page 
        await Shell.Current.GoToAsync("///NotificationPage");
    }

    private async void OnQRScanPageClicked(object sender, EventArgs e)
    {
        // Navigate to QR Scan Page or perform 
        await Shell.Current.GoToAsync("///CaretakerQR");
    }

    private async void OnMedicationListClicked(object sender, EventArgs e)
    {
        // Navigate to Medication List or perform 
        await Shell.Current.GoToAsync("///MedicationListPage");
    }

    private async void OnMedicationTrackerClicked(object sender, EventArgs e)
    {
        // Navigate to Medication Tracker or perform 
        await Shell.Current.GoToAsync("///MedicationTrackerPage");
    }

    private async void OnProfileIconTapped(object sender, EventArgs e)
    {
        // Navigate to the Profile page
        await Shell.Current.GoToAsync("///ProfilePage"); // Navigate to the ProfilePage
    }

    private async void OnSettingsIconTapped(object sender, EventArgs e)
    {
        // Navigate to the Settings page 
        await Shell.Current.GoToAsync("///SettingsPage"); // Navigate to the SettingsPage
    }

    private async void OnShiftPageClicked(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("///ShiftPage");
    }

    private async void OnTaskPageClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///TaskPage");
    }

    private async void OnPatientQRPageClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///PatientQRPage");
    }

}
