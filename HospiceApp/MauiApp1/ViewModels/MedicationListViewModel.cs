using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp1.Interfaces;
using MauiApp1.Models;
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

        [ObservableProperty]
        private ObservableCollection<ContactItem> _contactItemList;

        public event Action ContactsLoaded;

        public MedicationListViewModel(IRequestManager requestManager)
        {
            _requestManager = requestManager;
            _medicationDays = new ObservableGroupedCollection<string, MedData>();
            _contactItemList = new ObservableCollection<ContactItem>();
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

                    ContactItem contactItem = new ContactItem
                    {
                        PatientID = patientID,
                        Name = patientName,
                        Surname = patientSurname,
                        Medication = patientEntry.Value.Medication
                    };
                    ContactItemList.Add(contactItem);
                }

                // Trigger the event after loading data
                ContactsLoaded?.Invoke();
            }
            else
            {
                Debug.WriteLine("No medication data found.");
            }
        }
    }
}
