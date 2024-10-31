using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp1.Interfaces;
using MauiApp1.Models;
using MauiApp1.Models.Enums;
using MauiApp1.Models.PatientModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.ViewModels
{
    public class CaregiverShiftViewModel : ObservableObject
    {
        // need to set min and max date 
        private string _selectedDateString;
        public string SelectedDateString {
            get => _selectedDateString;
            set 
            {
                if (_selectedDateString != value)
                {
                    _selectedDateString = value;
                    OnPropertyChanged(nameof(SelectedDateString));
                }
            }
            
        }
        private DateTime _maximumDate;

        public DateTime MaximumDate
        {
            get => _maximumDate;
            set
            {
                if (_maximumDate != value)
                {
                    _maximumDate = value;
                    OnPropertyChanged(nameof(MaximumDate));
                }
            }
        }
        private DateTime _minimumDate;

        public DateTime MinimumDate
        {
            get => _minimumDate;
            set
            {
                if (_minimumDate != value)
                {
                    _minimumDate = value;
                    OnPropertyChanged(nameof(MinimumDate));
                }
            }
        }
        private readonly IRequestManager _requestManager;
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                OnSelectChange();
            }
        }

        private List<CaregiverShifts> _shifts;
        public ObservableCollection<CaregiverShifts> FilteredShifts { get; private set; }

        public CaregiverShiftViewModel(IRequestManager requestManager)
        {
            _requestManager = requestManager;
            _shifts = new List<CaregiverShifts>();
            _minimumDate = DateTime.Now;
            _maximumDate = DateTime.Now.AddDays(6);
            _selectedDateString = $"Shifts for {SelectedDate:MMMM dd, yyyy}";
            FilteredShifts = new ObservableCollection<CaregiverShifts>();
            // need to call API here
            GetData();
        }

        public async void GetData()
        {
            //string userID = await SecureStorage.GetAsync("UserID");
            //string token = await SecureStorage.GetAsync("Token");
            //_shifts = await _requestManager.GetCaregiverShiftsAsync(userID, token);

            // Dummy Data for testing purpose, API above
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

            // _shifts now have the shifts for the week
        }

        public void OnSelectChange()
        {
            // need to fix selected date string
            SelectedDateString = $"Shifts for {SelectedDate:MMMM dd, yyyy}";
            FilteredShifts.Clear();
            var Filtered = _shifts.Where(item => item.ShiftStart.Date.Equals(SelectedDate)).ToList();
            foreach (var item in Filtered)
            {
                FilteredShifts.Add(item);
            }
            OnPropertyChanged(nameof(FilteredShifts));
            // we only actually want to change when the date changes 
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
