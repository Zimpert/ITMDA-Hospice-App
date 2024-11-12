
using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp1.Interfaces;
using MauiApp1.Models;
using System.Collections.ObjectModel;

namespace MauiApp1.ViewModels
{
    public partial class MedicationListViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IContactRepo _contactRepo;
        public Action MedsLoaded;
        public string targetID;

        public MedicationListViewModel(IContactRepo contactRepo)
        {
            _contactRepo = contactRepo;
        }

        public List<Medication?> MedsItemList;

        public void GetMedications()
        {
            MedsItemList = _contactRepo.GetMedsByPatientID(targetID);
            MedsLoaded?.Invoke();

        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            targetID = query["PatientID"].ToString();
            GetMedications();
        }
    }
}
