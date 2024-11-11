using System.Collections.ObjectModel;
using MauiApp1.Interfaces;
using MauiApp1.Models;
using MauiApp1.ViewModels;
using Microsoft.Maui.Controls;

namespace MauiApp1.Views
{

    public partial class MedicationListPage : ContentPage
    {

        public MedicationListPage(MedicationListViewModel viewmodel)
        {
            InitializeComponent();
            BindingContext = viewmodel;
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
