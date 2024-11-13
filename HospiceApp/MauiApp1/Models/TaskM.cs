using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Interfaces;
using System.Text.Json.Serialization;

namespace MauiApp1.Models
{
    public partial class TaskM : ObservableObject
    {

        [ObservableProperty]
        private string dateDue;

        [ObservableProperty]
        private string taskID;

        [ObservableProperty]
        private string description;


    }
}
