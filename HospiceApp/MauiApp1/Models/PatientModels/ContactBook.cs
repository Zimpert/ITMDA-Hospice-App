using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models.PatientModels
{
    public class ContactBook
    {
        public string ContactBookID { get; set; }
        public List<PersonalContact>? PersonalContacts { get; set; }
        public List<MedicalContact>? MedicalContacts { get; set; }
    }
}
