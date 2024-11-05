using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models.PatientModels
{
    public class PatientMedication
    {
        public int PatientMedicationID { get; set; }
        public int PatientID { get; set; }
        public int MedicationID { get; set; }
        public int Dosage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public User Patient { get; set; }
        public List<Medication> Medications { get; set; }

        public List<MedicationDays> MedicationDays { get; set; }


    }
}