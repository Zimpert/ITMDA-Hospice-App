
using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp1.Interfaces;
using MauiApp1.Models;
using System.Collections.ObjectModel;

namespace MauiApp1.ViewModels
{
    public partial class MedicationListViewModel : ObservableObject
    {
        private readonly IContactRepo _contactRepo;
        public Action MedsLoaded;

        public MedicationListViewModel(IContactRepo contactRepo)
        {
            _contactRepo = contactRepo;
        }

        public List<Medication> MedsItemList;

        public void  GetMedications(string patientID)
        {
            MedsItemList =  _contactRepo.GetMedsByPatientID(patientID);
            MedsLoaded?.Invoke();

        }






    }
}
