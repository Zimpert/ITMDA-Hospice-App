using MauiApp1.Interfaces;
using MauiApp1.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MauiApp1.ViewModels
{
    public class ContactPageViewModel
    {
        private readonly IRequestManager _requestManager;
        private readonly IContactRepo _contactRepo;

        public ObservableCollection<ContactItem> ContactItemList => _contactRepo.ContactItemList;

        /// <summary>
        /// Event triggered when contact items are created.
        /// </summary>
        public Action ContactItemsCreated;

        public ContactPageViewModel(IRequestManager requestManager, IContactRepo contactRepo)
        {
            _requestManager = requestManager;
            _contactRepo = contactRepo;
            LoadMedication();
        }

        /// <summary>
        /// Loads the medication data for patients.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task LoadMedication()
        {
            try
            {
                var token = await SecureStorage.GetAsync("Token");

                if (string.IsNullOrEmpty(token))
                {
                    Debug.WriteLine("Token is null or empty.");
                    return;
                }

                // Fetch patient medication data
                var medicationResult = await _requestManager.GetPatientMedicationsAsync(token);

                if (medicationResult != null)
                {
                    // Iterate over the dictionary
                    foreach (var patientEntry in medicationResult)
                    {
                        // Patient ID
                        string patientID = patientEntry.Key;

                        // Patient Personal Info
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
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while loading medication: {ex.Message}");
            }
        }

        /// <summary>
        /// Event triggered when contacts are loaded.
        /// </summary>
        public event Action ContactsLoaded;
    }
}
