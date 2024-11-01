using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp1.Interfaces;
using MauiApp1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MauiApp1.ViewModels
{
    public partial class CaregiverShiftViewModel : ObservableObject
    {
        private readonly IRequestManager _requestManager;
        private List<CaregiverShifts> _shifts;

        [ObservableProperty]
        private string selectedDateString;

        [ObservableProperty]
        private DateTime maximumDate;

        [ObservableProperty]
        private DateTime minimumDate;

        [ObservableProperty]
        private DateTime selectedDate;

        public ObservableCollection<CaregiverShifts> FilteredShifts { get; private set; }

        public CaregiverShiftViewModel(IRequestManager requestManager)
        {
            _requestManager = requestManager;
            _shifts = new List<CaregiverShifts>();
            MinimumDate = DateTime.Now;
            MaximumDate = DateTime.Now.AddDays(6);
            SelectedDateString = $"Shifts for {SelectedDate:MMMM dd, yyyy}";
            FilteredShifts = new ObservableCollection<CaregiverShifts>();

            _ = GetDataAsync(); // Fire and forget the async call
        }

        public async Task GetDataAsync()
        {
            // Example of fetching from API; replace with actual API call
            //string userID = await SecureStorage.GetAsync("UserID");
            //string token = await SecureStorage.GetAsync("Token");
            //_shifts = await _requestManager.GetCaregiverShiftsAsync(userID, token);

            // Dummy data for testing
            _shifts = new List<CaregiverShifts>
            {
                new CaregiverShifts { ShiftStart = new DateTime(2024, 10, 30), PatientName = "Shift 1" },
                new CaregiverShifts { ShiftStart = new DateTime(2024, 10, 31), PatientName = "Shift 2" },
                new CaregiverShifts { ShiftStart = new DateTime(2024, 10, 29), PatientName = "Shift 3" },
                new CaregiverShifts { ShiftStart = new DateTime(2024, 10, 29), PatientName = "Shift 4" }
            };

            foreach (var item in _shifts)
            {
                item.Time = item.ShiftStart.ToString("HH:mm") + " - " + item.ShiftEnd.ToString("HH:mm");
            }
        }

        partial void OnSelectedDateChanged(DateTime value)
        {
            SelectedDateString = $"Shifts for {value:MMMM dd, yyyy}";
            FilteredShifts.Clear();
            var filtered = _shifts.Where(item => item.ShiftStart.Date.Equals(SelectedDate)).ToList();
            foreach (var item in filtered)
            {
                FilteredShifts.Add(item);
            }
        }
    }
}
