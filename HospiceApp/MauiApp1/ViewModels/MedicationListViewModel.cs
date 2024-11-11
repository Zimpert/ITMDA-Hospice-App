using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp1.Interfaces;
using MauiApp1.Models;
using MauiApp1.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MauiApp1.ViewModels
{
    public partial class MedicationListViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IRequestManager _requestManager;
        private readonly ContactService _cService;

        [ObservableProperty]
        private ObservableCollection<Medication> _filteredMeds;

        private string targetID;

        public MedicationListViewModel(IRequestManager requestManager, ContactService cService)
        {
            _requestManager = requestManager;
            _filteredMeds = new ObservableCollection<Medication>();
            _cService = cService;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            targetID = query["patientID"].ToString();
            Debug.WriteLine($"TARGET ID IS {targetID}");
            LoadMedications();
        }

        public void LoadMedications()
        {
            _filteredMeds.Clear();

            // Get the contact for the specific patient
            var contact = _cService.ContactItemList.FirstOrDefault(c => c.PatientID == targetID);

            if (contact != null)
            {
                // Filter medications for the specific patient
                foreach (var med in contact.Medication)
                {
                    _filteredMeds.Add(med);
                }
            }

        }

        }
}
