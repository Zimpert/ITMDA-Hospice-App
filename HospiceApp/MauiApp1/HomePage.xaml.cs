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
        await DisplayAlert("Navigation", "View Carer button clicked", "OK");
    }

    private async void OnShiftPageClicked(object sender, EventArgs e)
    {
        // Navigate to Shift Page 
        await DisplayAlert("Navigation", "Shift Page button clicked", "OK");
    }

    private async void OnNotificationPageClicked(object sender, EventArgs e)
    {
        // Navigate to Notification Page 
        await DisplayAlert("Navigation", "Notification Page button clicked", "OK");
    }

    private async void OnQRScanPageClicked(object sender, EventArgs e)
    {
        // Navigate to QR Scan Page or perform 
        await DisplayAlert("Navigation", "QR Scan Page button clicked", "OK");
    }

    private async void OnMedicationListClicked(object sender, EventArgs e)
    {
        // Navigate to Medication List or perform 
        await DisplayAlert("Navigation", "Medication List button clicked", "OK");
    }

    private async void OnMedicationTrackerClicked(object sender, EventArgs e)
    {
        // Navigate to Medication Tracker or perform 
        await DisplayAlert("Navigation", "Medication Tracker button clicked", "OK");
    }
}
