using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Models.PatientModels;
using System;

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
         List<Medication>? medication
            ;
        public ContactItem()
        {
            
        }

        [RelayCommand] public async Task NavToMedList()
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "PatientID", PatientID }
            };
            await Shell.Current.GoToAsync("//MedicationListPage", navigationParameter);
        }



    }
}
