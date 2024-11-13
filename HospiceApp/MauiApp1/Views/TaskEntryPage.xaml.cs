using MauiApp1.ViewModels;
using Microsoft.Maui.Controls;

namespace MauiApp1.Views
{
    public partial class TaskEntryPage : ContentPage
    {
        public TaskEntryPage(TaskViewModel tsvm, string targetID)
        {
            InitializeComponent();
            BindingContext = new TaskEntryViewModel(tsvm, targetID);
        }

        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}