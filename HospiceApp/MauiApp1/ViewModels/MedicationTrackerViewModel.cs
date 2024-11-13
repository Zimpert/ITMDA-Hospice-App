using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Interfaces;
using MauiApp1.Models;
using System.Collections.ObjectModel;

namespace MauiApp1.ViewModels
{
    public partial class MedicationTrackerViewModel : ObservableObject, IQueryAttributable
    {

        public string PatientID;
        private readonly IRequestManager _requestManager;
        private readonly IContactRepo _repo;

        [ObservableProperty]
        private ObservableCollection<Medication> medications;

        [ObservableProperty]
        private string medicationID;
        public MedicationTrackerViewModel(IRequestManager requestManager, IContactRepo contactRepo)
        {
            _requestManager = requestManager;
            _repo = contactRepo;
            Medications = new();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            // Id of the patients stuff we need to load, initial load can be used with contact repo, cos all the meds are stored in there
            // then after all additional loading must be done 
            PatientID = query["PatientID"].ToString();

            _ = LoadMeds(); // use contact repo, upon completion use the API
        }

        private async Task LoadMeds()
        {
            // call API only has to happen after a med is logged 
            // on load isnt required ?? 
            var MedList = _repo.GetMedsByPatientID(PatientID);
            // only display where the day is today 

            Medications.Clear();
            foreach (var item in MedList)
            {
                if (item.Day.Equals(DateTime.Now.DayOfWeek.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    //
                {
                    Medications.Add(item);
                }

            }
        }

        [RelayCommand]
        private async Task MedicationTaken(string MedID)
        {
            // call the log api, then call get medication, then load it 
            var token = await SecureStorage.GetAsync("Token");
            await _requestManager.MedLog(token, MedID);
            // if it's been taken we need to log that somehow and not re-display it!
            await GetMedications();
        }

        public async Task GetMedications()
        {

            var token = await SecureStorage.GetAsync("Token");
            Medications.Clear();
            var NewMedList = await _requestManager.GetPatientMedsONLY(token, PatientID);
            foreach (var item in NewMedList)
            {
                if (item.Day.Equals(DateTime.Now.DayOfWeek.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    Medications.Add(item);
                }
            }
        }

    }

}
