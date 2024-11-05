using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models.PatientModels
{
    public class MedicationDays
    {
        public string PatientMedicationDayID { get; set; }
        public string MedicationName { get; set; }
        public string Day { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Dosage { get; set; }
        public string Interactions { get; set; }
    }
}
