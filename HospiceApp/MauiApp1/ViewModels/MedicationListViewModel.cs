using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp1.Interfaces;
using MauiApp1.Models.PatientModels;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static MauiApp1.Services.RequestManager;

namespace MauiApp1.ViewModels
{
    public partial class MedicationListViewModel : ObservableObject
    {
        private readonly IRequestManager _requestManager;

        [ObservableProperty]
        private ObservableGroupedCollection<string, MedData> _medicationDays;

        public MedicationListViewModel(IRequestManager requestManager)
        {
            _requestManager = requestManager;
            _medicationDays = new ObservableGroupedCollection<string, MedData>();
            LoadMedication();
        }

        public async Task LoadMedication()
        {
            // Get the token from SecureStorage
            var token = await SecureStorage.GetAsync("Token");

            // Fetch patient medication data from the API
            var medicationResult = await _requestManager.GetPatientMedicationsAsync(token);

            if (medicationResult != null)
            {
                // Iterate over the dictionary to log the details to Debug.WriteLine
                foreach (var patientEntry in medicationResult)
                {
                    // Patient ID
                    string patientID = patientEntry.Key;

                    // Patient Info
                    var patientInfo = patientEntry.Value.PatientInfo;
                        
                    // PatientInfo { "PatientName" : "Name", "PatientSurname" : "Suranme" }

                    string patientName = patientInfo?.PatientName ?? "Unknown Name";
                    string patientSurname = patientInfo?.PatientSurname ?? "Unknown Surname";

                    // Log patient info
                    Debug.WriteLine($"Patient ID: {patientID}");
                    Debug.WriteLine($"Patient Name: {patientName}");
                    Debug.WriteLine($"Patient Surname: {patientSurname}");

                    // Iterate over the medications for this patient
                    foreach (var medication in patientEntry.Value.Medication)
                        // Medication { [ ]}
                    {
                        // Log medication info
                        Debug.WriteLine($"Medication Name: {medication.MedicationName}");
                        Debug.WriteLine($"Description: {medication.Description}");
                        Debug.WriteLine($"Dosage: {medication.Dosage}");
                        Debug.WriteLine($"Frequency: {medication.Frequency}");
                        Debug.WriteLine($"Day: {medication.Day}");
                        Debug.WriteLine($"Start Date: {medication.StartDate}");
                        Debug.WriteLine($"End Date: {medication.EndDate}");
                        // PatientName:
                            // -> Paracetamol
                            // -> Lexapro
                    }
                }
            }
            else
            {
                Debug.WriteLine("No medication data found.");
            }
        }
    }
}
