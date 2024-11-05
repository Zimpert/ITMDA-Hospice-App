using MauiApp1.PartialViews;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Globalization;

namespace MauiApp1.Views
{
    public partial class TaskPage : ContentPage
    {
        public TaskPage()
        {
            InitializeComponent();
            LoadTasksForSelectedDate(DatePicker.Date);
            DatePicker.DateSelected += OnDateSelected;
        }

        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            LoadTasksForSelectedDate(e.NewDate);
        }

        private void LoadTasksForSelectedDate(DateTime selectedDate)
        {
            // Clear existing tasks
            TaskListContainer.Children.Clear();

            // Load tasks (this is a mock data example; replace with actual data)
            var tasks = GetTasksForDate(selectedDate);

            foreach (var task in tasks)
            {
                var taskItemView = new TaskItemView
                {
                    TaskTitle = task.Title,
                    TaskDescription = task.Description,
                    IsCompleted = task.IsCompleted
                };
                TaskListContainer.Children.Add(taskItemView);
            }
        }

        private List<TaskItem> GetTasksForDate(DateTime date)
        {
            // Placeholder data; in a real app, fetch tasks from a database or API based on the date
            return new List<TaskItem>
            {
                new TaskItem { Title = "Medication", Description = "Levopoda", IsCompleted = false },
                new TaskItem { Title = "Appointment", Description = "Doctor's Appointment", IsCompleted = false },
                new TaskItem { Title = "Check-up", Description = "Heart-rate and Pain Assessment", IsCompleted = false }
            };
        }
    }

    public class TaskItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}