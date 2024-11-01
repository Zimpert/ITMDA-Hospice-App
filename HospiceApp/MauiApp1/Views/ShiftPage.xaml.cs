using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using MauiApp1.ViewModels;
using MauiApp1.Services;
using MauiApp1.Interfaces;

namespace MauiApp1
{
    public partial class ShiftsPage : ContentPage
    {
        //public ObservableCollection<Shift> ShiftsForSelectedDate { get; set; }
        //public DateTime SelectedDate { get; set; }
        //public string SelectedDateString { get; set; }

        //public DateTime MinimumDate { get; set; } = new DateTime(2024, 1, 1);
        //public DateTime MaximumDate { get; set; } = new DateTime(2024, 12, 31);

        public ShiftsPage()
        {
            InitializeComponent();
            //SelectedDate = DateTime.Today;
            //ShiftsForSelectedDate = new ObservableCollection<Shift>();
            //UpdateShiftsForSelectedDate();
            var requestManager = MauiProgram.ServiceProvider.GetService<IRequestManager>();

            BindingContext = new CaregiverShiftViewModel(requestManager);
        }

        //private void UpdateShiftsForSelectedDate()
        //{
        //    SelectedDateString = $"Shifts for {SelectedDate:MMMM dd, yyyy}";
        //    ShiftsForSelectedDate.Clear();

        //    // Example data based on the selected date
        //    if (SelectedDate.DayOfWeek == DayOfWeek.Friday)
        //    {
        //        ShiftsForSelectedDate.Add(new Shift { Title = "Morning Shift", Time = "08:00 AM - 12:00 PM" });
        //        ShiftsForSelectedDate.Add(new Shift { Title = "Evening Shift", Time = "01:00 PM - 05:00 PM" });
        //    }
        //    else if (SelectedDate.DayOfWeek == DayOfWeek.Monday)
        //    {
        //        ShiftsForSelectedDate.Add(new Shift { Title = "Morning Shift", Time = "09:00 AM - 12:00 PM" });
        //    }
        //    else
        //    {
        //        ShiftsForSelectedDate.Add(new Shift { Title = "Day Shift", Time = "10:00 AM - 06:00 PM" });
        //    }

        //    OnPropertyChanged(nameof(ShiftsForSelectedDate));
        //    OnPropertyChanged(nameof(SelectedDateString));
        //}

        // Action when the DatePicker changes the selected date
        //private void OnDateChanged(object sender, DateChangedEventArgs e)
        //{
        //    SelectedDate = e.NewDate;
        //    UpdateShiftsForSelectedDate();
        //}
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

    public class Shift
    {
        public string Title { get; set; }
        public string Time { get; set; }
    }
}
