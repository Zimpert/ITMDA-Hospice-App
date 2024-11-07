using MauiApp1.Models.PatientModels;
using System;

namespace MauiApp1.Models
{
    public class ContactItem
    {

        public string PatientID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<Medication>? Medication { get; set; }

        public ContactItem()
        {
            
        }

    }
}
