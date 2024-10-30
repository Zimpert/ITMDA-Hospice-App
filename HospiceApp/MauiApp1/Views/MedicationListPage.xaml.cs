using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace MauiApp1
{
    public partial class MedicationListPage : ContentPage
    {
        public MedicationListPage()
        {
            InitializeComponent();
            BindingContext = new MedicationListViewModel();
        }

        // Event handler for the Add Medication button
        private void OnAddMedicationClicked(object sender, EventArgs e)
        {
            // Handle the action for adding a new medication
            DisplayAlert("Add Medication", "Button clicked to add a new medication", "OK");
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

    // ViewModel for Medication List Page
    public class MedicationListViewModel
    {
        public ObservableCollection<Medication> Medications { get; set; }

        public MedicationListViewModel()
        {
            Medications = new ObservableCollection<Medication>
            {
                new Medication { Name = "Ibuprofen", Dosage = "Twice a day" },
                new Medication { Name = "Amoxicillin", Dosage = "Once a day" },
                new Medication { Name = "Metformin", Dosage = "Once a day" },
                new Medication { Name = "Atorvastatin", Dosage = "Once a day" }
            };
        }
    }



    // Model for Medication Item
    public class Medication
    {
        public string Name { get; set; }
        public string Dosage { get; set; }
    }
}
