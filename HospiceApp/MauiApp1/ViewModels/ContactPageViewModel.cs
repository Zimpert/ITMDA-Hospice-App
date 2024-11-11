using CommunityToolkit.Mvvm.Input;
using MauiApp1.Interfaces;
using MauiApp1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.ViewModels
{
    public class ContactPageViewModel
    {
        private readonly IRequestManager _requestManager;
        private readonly IContactRepo _contactRepo;
        public ObservableCollection<ContactItem> ContactItemList => _contactRepo.ContactItemList;

        public Action ContactItemsCreated;

        public ContactPageViewModel(IRequestManager requestManager, IContactRepo contactRepo)
        {
            _requestManager = requestManager;
            _contactRepo = contactRepo;
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

                    string patientName = patientInfo?.PatientName ?? "Unknown Name";
                    string patientSurname = patientInfo?.PatientSurname ?? "Unknown Surname";

                    ContactItem contactItem = new ContactItem
                    {
                        PatientID = patientID,
                        Name = patientName,
                        Surname = patientSurname,
                        Medication = patientEntry.Value.Medication
                    };

                    _contactRepo.ContactItemList.Add(contactItem);
                }

                // Trigger the event after loading data
                ContactItemsCreated?.Invoke();
            }
            else
            {
                Debug.WriteLine("No medication data found.");
            }
        }

        public event Action ContactsLoaded;
    }
}
