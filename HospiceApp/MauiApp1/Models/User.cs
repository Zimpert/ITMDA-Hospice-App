using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp1.Models.PatientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    public partial class User : ObservableObject
    {
        required public string UserID { get; set; }
        required public string Role { get; set; }
        [ObservableProperty]
        required public string name;
        [ObservableProperty]
        required public string surname;
        [ObservableProperty]
        public string? phoneNo;
        [ObservableProperty]
        
        required public string email;
        [ObservableProperty]
        public string? address;
        public string? Token { get; set; }
        public ContactBook? ContactBook { get; set; }
        public List<PatientMedication>? PatientMedications { get; set; }
        public List<Condition>? PatientConditions { get; set; }



    }
}
