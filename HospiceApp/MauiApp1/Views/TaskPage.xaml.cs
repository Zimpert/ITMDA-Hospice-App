using MauiApp1.Models;
using MauiApp1.PartialViews;
using MauiApp1.ViewModels;

namespace MauiApp1.Views
{
    public partial class TaskPage : ContentPage
    {

        private readonly TaskViewModel viewModelModel;
        public TaskPage(TaskViewModel TASKVIEW)
        {
            InitializeComponent();
            BindingContext = TASKVIEW;

            viewModelModel = TASKVIEW;
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


        private async void AddTask(object sender, EventArgs e)
        {
            // Navigate to the Settings page 
            var taskEntryPage = new TaskEntryPage(viewModelModel, viewModelModel.targetID);
            await Navigation.PushModalAsync(taskEntryPage);
        }
    }
}