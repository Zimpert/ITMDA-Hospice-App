using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp1.Interfaces;
using MauiApp1.Models.PatientModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.ViewModels
{
    public partial class MedicationListViewModel : ObservableObject
    {
        private readonly IRequestManager _requestManager;

        [ObservableProperty]
        private ObservableCollection<MedicationDays> _medicationDays;

        public MedicationListViewModel(IRequestManager requestManager)
        {
            _requestManager = requestManager;
            _medicationDays = new ObservableCollection<MedicationDays>();
        }

        public async Task LoadMedication()
        {
            var token = await SecureStorage.GetAsync("Token");
            var userId = await SecureStorage.GetAsync("UserID");
            var medicationResult = await _requestManager.GetPatientMedicationsAsync(userId, token);

            if (medicationResult != null)
            {
                MedicationDays = new ObservableCollection<MedicationDays>(medicationResult);
            }
            else
            {
                MedicationDays = new ObservableCollection<MedicationDays>();
            }
        }
    }
}
