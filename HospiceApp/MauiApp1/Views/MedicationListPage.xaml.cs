using System.Collections.ObjectModel;
using MauiApp1.Interfaces;
using MauiApp1.PartialViews;
using MauiApp1.ViewModels;
using Microsoft.Maui.Controls;

namespace MauiApp1.Views
{
    public partial class MedicationListPage : ContentPage

    {
        private readonly MedicationListViewModel _viewModel;
        public MedicationListPage(MedicationListViewModel viewmodel)
        {
            InitializeComponent();
            BindingContext = viewmodel;
            _viewModel = viewmodel;
            _viewModel.MedsLoaded += OnMedsLoaded;
        }

        private void OnMedsLoaded()
        {
            LoadMeds();
        }

        public void LoadMeds()
        {
            // Clear existing contacts
            MedsListContainer.Children.Clear();


            // Add each contact to the container
            if (_viewModel.MedsItemList != null)
            {
                foreach (var medication in _viewModel.MedsItemList)
                {
                    var medicationItemView = new MedicationItemView
                    {
                        BindingContext = medication
                    };
                    MedsListContainer.Children.Add(medicationItemView);
                }
            }
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
}
