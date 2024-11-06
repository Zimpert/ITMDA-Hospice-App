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
        public string UserID { get; set; }
        public string Role { get; set; }
        [ObservableProperty]
        public string name;
        [ObservableProperty]
        public string surname;
        [ObservableProperty]
        public string? phoneNo;
        [ObservableProperty]
        
        public string email;
        [ObservableProperty]
        public string? address;
        public string? Token { get; set; }
        public ContactBook? ContactBook { get; set; }
        public List<Medication>? PatientMedications { get; set; }
        public List<Condition>? PatientConditions { get; set; }



    }
}
