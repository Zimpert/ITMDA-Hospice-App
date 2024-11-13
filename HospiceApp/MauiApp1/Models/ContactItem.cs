using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Models.PatientModels;
using System;

namespace MauiApp1.Models
{
    public partial class ContactItem : ObservableObject
    {
        [ObservableProperty]
        string patientID;
        [ObservableProperty]
        string name;
        [ObservableProperty]
        string surname;
        [ObservableProperty]
        List<Medication>? medication;

        public ContactItem()
        {

        }

        /// <summary>
        /// Navigates to the medication list page for the current patient.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [RelayCommand]
        public async Task NavToMedList()
        {
            var navigationParameter = new Dictionary<string, object>
                {
                    { "PatientID", PatientID }
                };
            await Shell.Current.GoToAsync("//MedicationListPage", navigationParameter);
        }

        /// <summary>
        /// Navigates to the patient details page for the current patient.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [RelayCommand]
        public async Task NavToPatientDetails()
        {
            var navigationParameter = new Dictionary<string, object>
                {
                    { "PatientID", PatientID }
                };
            await Shell.Current.GoToAsync("//PatientDetailsPage", navigationParameter);
        }

        /// <summary>
        /// Navigates to the patient tasks page for the current patient.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [RelayCommand]
        public async Task NavToPatientTasks()
        {
            var navigationParameter = new Dictionary<string, object>
                {
                    { "PatientID", PatientID }
                };
            await Shell.Current.GoToAsync("//TaskPage", navigationParameter);
        }
    }
}
