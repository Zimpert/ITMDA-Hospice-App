using System.Collections.ObjectModel;
using MauiApp1.Interfaces;
using MauiApp1.ViewModels;
using Microsoft.Maui.Controls;

namespace MauiApp1.Views
{
    public partial class MedicationListPage : ContentPage
    {
        private readonly IRequestManager _requestManager;
        private readonly MedicationListViewModel _viewModel;
        public MedicationListPage(IRequestManager requestManager, MedicationListViewModel viewmodel)
        {
            InitializeComponent();
            //BindingContext = new MedicationListViewModel();
            BindingContext = viewmodel;
            _requestManager = requestManager;
            _viewModel = viewmodel;

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadMedication();

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
             
            await Shell.Current.GoToAsync("///SettingsPage"); // Navigate to the SettingsPage
        }
    }

    //// ViewModel for Medication List Page
    //public class MedicationListViewModel
    //{
    //    public ObservableCollection<Medication> Medications { get; set; }

    //    public MedicationListViewModel()
    //    {
    //        Medications = new ObservableCollection<Medication>
    //        {
    //            new Medication { Name = "Ibuprofen", Dosage = "Twice a day" },
    //            new Medication { Name = "Amoxicillin", Dosage = "Once a day" },
    //            new Medication { Name = "Metformin", Dosage = "Once a day" },
    //            new Medication { Name = "Atorvastatin", Dosage = "Once a day" }
    //        };
    //    }
    //}



    //// Model for Medication Item
    //public class Medication
    //{
    //    public string Name { get; set; }
    //    public string Dosage { get; set; }
    //}
}
