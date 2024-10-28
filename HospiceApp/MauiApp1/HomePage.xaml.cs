namespace MauiApp1;
using Microsoft.Maui.Controls;
using System;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }
    // Event handlers 
    private async void OnViewCarerClicked(object sender, EventArgs e)
    {
        // Navigate to View Carer page 
        await Shell.Current.GoToAsync("///ViewCarerPage"); 
    }

    private async void OnShiftPageClicked(object sender, EventArgs e)
    {
        // Navigate to Shift Page 
        await Shell.Current.GoToAsync("///ShiftPage");
    }

    private async void OnNotificationPageClicked(object sender, EventArgs e)
    {
        // Navigate to Notification Page 
        await Shell.Current.GoToAsync("///NotificationPage");
    }

    private async void OnQRScanPageClicked(object sender, EventArgs e)
    {
        // Navigate to QR Scan Page or perform 
        await Shell.Current.GoToAsync("///QRScanPage");
    }

    private async void OnMedicationListClicked(object sender, EventArgs e)
    {
        // Navigate to Medication List or perform 
        await Shell.Current.GoToAsync("///MedicationListTracker");
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

}
