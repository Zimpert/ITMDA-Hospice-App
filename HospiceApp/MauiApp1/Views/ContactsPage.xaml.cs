using MauiApp1.Models;
using MauiApp1.PartialViews;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using MauiApp1;
using MauiApp1.Interfaces;
using MauiApp1.ViewModels;

namespace MauiApp1.Views
{
    public partial class ContactsPage : ContentPage
    {

        public ContactsPage(ContactViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
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
}
