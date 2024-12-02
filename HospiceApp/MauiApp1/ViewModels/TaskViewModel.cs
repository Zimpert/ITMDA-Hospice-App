using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Interfaces;
using MauiApp1.Models;
using MauiApp1.Views;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MauiApp1.ViewModels
{
    public partial class TaskViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IRequestManager _requestManager;
        public string targetID;
        public string taskID;
        public Action TasksLoaded;
        [ObservableProperty]
        private ObservableCollection<TaskM> tasks = new();


        private string dateDue;


        private string description;

        public TaskViewModel(IRequestManager reqMan)
        {
            _requestManager = reqMan;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            targetID = query["PatientID"].ToString();
            
            _ = LoadTasks();
        }

        private async Task LoadTasks()
        {
            // call API and load it into a list and then fire an event
            // will have to call the api but you need to extract it in a very specific way
            var TasksList = await _requestManager.GetPatientTasks(targetID);
            var Sorted = TasksList.OrderBy(item => item.DateDue).ToList();

            Tasks.Clear();
            foreach (var item in Sorted)
            {
                Tasks.Add(item);
            }
        }

        [RelayCommand]
        public async Task TaskCompletion(string taskID)
        {
            if (string.IsNullOrEmpty(taskID))
                return;

            // Logic to mark the task as complete using the taskID
            var token = await SecureStorage.GetAsync("Token");
            await _requestManager.TaskLog(token, taskID);

            // Reload or update tasks after marking as complete
            await LoadTasks();
        }

        [RelayCommand]
        public async Task SaveTask()
        {
            // Logic to save the task (e.g., add to a list or send to a database)

            // Close the modal after saving
            //await Application.Current.MainPage.Navigation.PopModalAsync();

        }

        public async Task UpdateValues(string DueDateS, string Desc, string UserID)
        {
            dateDue = DueDateS;
            description = Desc;
            targetID = UserID;
            var Token = await SecureStorage.GetAsync("Token");
            await _requestManager.AddPatientTasks(dateDue, description, Token, targetID);
            LoadTasks();
        }

        [RelayCommand]
        public async void AddTask()
        {
            
            var Token = await SecureStorage.GetAsync("Token");
            await _requestManager.AddPatientTasks("2024-03-01 12:15:00", "Just added per jano request", Token, targetID);
            LoadTasks();

        }

    }
}
