using MauiApp1.Models;
using MauiApp1.PartialViews;
using MauiApp1.ViewModels;

namespace MauiApp1.Views
{
    public partial class TaskPage : ContentPage
    {
        public TaskPage(TaskViewModel tsvm)
        {
            InitializeComponent();
            BindingContext = tsvm;

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