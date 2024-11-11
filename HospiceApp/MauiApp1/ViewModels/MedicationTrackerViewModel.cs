using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace MauiApp1.ViewModels
{
    public class MedicationTrackerViewModel : ObservableObject
    {
        private ObservableCollection<MedicationTrackerItem> medications;
        public ObservableCollection<MedicationTrackerItem> Medications
        {
            get => medications;
            set => SetProperty(ref medications, value);
        }

        public MedicationTrackerViewModel()
        {
            Medications = new ObservableCollection<MedicationTrackerItem>();
        }
    }

    public class MedicationTrackerItem : ObservableObject
    {
        public string MedicationName { get; set; }
        public string Dosage { get; set; }

        private bool isTaken;
        public bool IsTaken
        {
            get => isTaken;
            set => SetProperty(ref isTaken, value);
        }
    }
}
