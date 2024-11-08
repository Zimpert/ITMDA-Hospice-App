using MauiApp1.ViewModels;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace MauiApp1.Views
{
    public partial class MedicationTrackerPage : ContentPage
    {
        public MedicationTrackerPage()
        {
            InitializeComponent();

            // Sample data for testing
            var medications = new List<MedicationTrackerItem>
            {
                new MedicationTrackerItem { MedicationName = "Ibuprofen", Dosage = "200mg", IsTaken = false },
                new MedicationTrackerItem { MedicationName = "Amoxicillin", Dosage = "500mg", IsTaken = true },
                new MedicationTrackerItem { MedicationName = "Metformin", Dosage = "500mg", IsTaken = false },
                new MedicationTrackerItem { MedicationName = "Atorvastatin", Dosage = "20mg", IsTaken = true },

            }; 


            BindingContext = new MedicationTrackerViewModel
            {
                Medications = new ObservableCollection<MedicationTrackerItem>(medications)
            };
        }

        private async void OnSettingsIconTapped(object sender, EventArgs e)
        {
            // Navigate to the Settings page 
            await Shell.Current.GoToAsync("///SettingsPage"); // Navigate to the SettingsPage
        }

        private async void OnBackButtonTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///HomePage"); // Navigate back to the previous page
        }
    }
}
