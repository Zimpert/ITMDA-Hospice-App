using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Interfaces;
using MauiApp1.Models;
using MauiApp1.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace MauiApp1.ViewModels
{

    public partial class ContactViewModel : ObservableObject
    {
        private readonly IRequestManager _reqManager;
        private readonly ContactService _contService;


        [ObservableProperty]
        private ObservableCollection<ContactItem> _contactItemList;

        [ObservableProperty]
        private ContactItem _selectedContact;

        public ICommand TakeToDetailsPageCommand => new AsyncRelayCommand<ContactItem>(TakeToDetailsPage);

        public ContactViewModel(IRequestManager reqManager, ContactService contactService) 
        {
            _reqManager = reqManager;
            _contService = contactService;
            _contactItemList = new ObservableCollection<ContactItem>();
            LoadContact();
        }

        private async Task TakeToDetailsPage(ContactItem contact)
        {
            _contService.SetContactList(ContactItemList);
            var patientID = contact.PatientID;
            //var contactList = _contactItemList;
            await Shell.Current.GoToAsync($"///MedicationListPage?patientID={patientID}");
        }

        public async Task LoadContact()
        {
            // Get the token from SecureStorage
            var token = await SecureStorage.GetAsync("Token");

            // Fetch patient medication data from the API
            var medicationResult = await _reqManager.GetPatientMedicationsAsync(token);

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
                // ContactsLoaded?.Invoke();
            }
            else
            {
                Debug.WriteLine("No medication data found.");
            }
        }


    }
}
