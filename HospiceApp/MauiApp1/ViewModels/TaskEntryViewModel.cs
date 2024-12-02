using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MauiApp1.ViewModels
{
    public partial class TaskEntryViewModel : ObservableObject
    {
        private readonly TaskViewModel _viewModel;

        [ObservableProperty]
        private DateTime dateDue;

        [ObservableProperty]
        private string description;

        private string userID;


        public TaskEntryViewModel(TaskViewModel tsvm, string UserID)
        {
            DateDue = DateTime.Today; // Default to today
            _viewModel = tsvm;
            userID = UserID;
        }

        [RelayCommand]
        private async void SaveTask()
        {
            // Logic to save the task (e.g., call a service, update a shared list, etc.)
            await _viewModel.UpdateValues(DateDue.ToString("yyyy-MM-dd HH:mm:ss").Replace("/", "-"), Description, userID);
            // Close the modal after saving
            await Application.Current.MainPage.Navigation.PopModalAsync();
            
            
        }
    }
}
