using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp1.Interfaces;
using MauiApp1.Models;
using MauiApp1.Models.Enums;
using MauiApp1.Models.PatientModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Dispatching;

namespace MauiApp1.ViewModels
{
    public class CaregiverShiftViewModel : ObservableObject
    {
        private string _selectedDateString;
        public string SelectedDateString
        {
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
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged(nameof(SelectedDate));
                    OnSelectChange();
                }
            }
        }

        private List<CaregiverShifts?> _shifts;
        public ObservableCollection<CaregiverShifts> FilteredShifts { get; private set; }

        public CaregiverShiftViewModel(IRequestManager requestManager)
        {
            _requestManager = requestManager;
            _shifts = new List<CaregiverShifts?>();
            _minimumDate = DateTime.Now;
            _maximumDate = DateTime.Now.AddDays(6);
            _selectedDate = DateTime.Now;
            _selectedDateString = $"Shifts for {SelectedDate:MMMM dd, yyyy}";
            FilteredShifts = new ObservableCollection<CaregiverShifts>();

            GetData();
        }

        public async Task GetData()
        {
            try
            {

                // Dummy data for testing purposes
                await Task.Run(async () =>
                {
                    // userID
                    var userID = await SecureStorage.GetAsync("UserID");
                    var token = await SecureStorage.GetAsync("Token");
                    _shifts = await _requestManager.GetCaregiverShiftsAsync(userID, token);

                    foreach (var item in _shifts)
                    {
                        item.Time = $"{item.ShiftStart:HH:mm} - {item.ShiftEnd:HH:mm}";
                        Debug.WriteLine(item.ShiftStart);
                        Debug.WriteLine("ITEM");
                        Debug.WriteLine(item.Name);
                    }
                });

                UpdateFilteredShifts();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading data: {ex.Message}");
            }
        }

        private void UpdateFilteredShifts()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                FilteredShifts.Clear();
                var filteredItems = _shifts
                    .Where(item => DateTime.TryParse(item.ShiftStart, out var shiftStartDate) && shiftStartDate.Date == SelectedDate.Date)
                    .ToList();

                foreach (var item in filteredItems)
                {
                    Debug.WriteLine("FOREACH RAN");
                    FilteredShifts.Add(item);
                }

                Debug.WriteLine($"FilteredShifts count after update: {FilteredShifts.Count}");
            });

            OnPropertyChanged(nameof(FilteredShifts));
        }


        public void OnSelectChange()
        {
            try
            {
                SelectedDateString = $"Shifts for {SelectedDate:MMMM dd, yyyy}";
                UpdateFilteredShifts();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating shifts: {ex.Message}");
            }
        }
    }
}
