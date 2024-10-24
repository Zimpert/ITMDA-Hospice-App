using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models.PatientModels
{
    public class Patient : User
    {
        public int PatientID { get; set; }

        public ContactBook ContactBook { get; set; }
        public List<PatientMedication>? PatientMedications { get; set; }
        public List<Condition>? PatientConditions { get; set; }

    }
}
