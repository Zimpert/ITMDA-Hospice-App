using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MauiApp1.Models
{
    public partial class ContactItem : ObservableObject
    {
        [ObservableProperty]
        string patientID;
        [ObservableProperty]
         string name;
        [ObservableProperty]
         string surname;
        [ObservableProperty]
         List<Medication>? medication;

        [RelayCommand] 
        public async Task NavToMedList()
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "PatientID", PatientID }
            };
            await Shell.Current.GoToAsync("//MedicationListPage", navigationParameter);
        }



    }
}
