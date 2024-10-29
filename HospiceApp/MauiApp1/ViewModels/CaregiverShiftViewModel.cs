using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp1.Interfaces;
using MauiApp1.Models;
using MauiApp1.Models.Enums;
using MauiApp1.Models.PatientModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.ViewModels
{
    public class CaregiverShiftViewModel :ObservableObject
    {
        private readonly IRequestManager _requestManager;
        private DateTime _selectedDate = DateTime.Now;

        private ObservableCollection<CaregiverShifts> _shifts;
        public ObservableCollection<CaregiverShifts> FilteredShifts { get; private set; }
        public ObservableCollection<DateTime> DaysInWeek { get; private set; }

        public CaregiverShiftViewModel(IRequestManager requestManager)
        {
            _requestManager = requestManager;
            _shifts = new ObservableCollection<CaregiverShifts>();
            FilteredShifts = new ObservableCollection<CaregiverShifts>();
            LoadDaysOfWeek();
        }

        // need a method to load the dates for the week
        public void LoadDaysOfWeek()
        {
            var startDay = DateTime.Now;
            
            while (startDay.DayOfWeek != DayOfWeek.Sunday)
            {
                DaysInWeek.Add(startDay.Date);
                startDay.AddDays(1);
            }
        }

        public void onSelectChange()
        {

        }

        public void FilterDate()
        {
            var Filtered = _shifts.Where(item => item.ShiftStart.Date == _selectedDate);
            foreach (var item in Filtered)
            {
                FilteredShifts.Add(item);
            }
            OnPropertyChanged(nameof(FilteredShifts));
        }

    }
}
