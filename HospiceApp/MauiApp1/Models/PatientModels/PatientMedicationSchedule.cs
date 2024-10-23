using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models.PatientModels
{
    public class PatientMedicationSchedule
    {
        public int PatientMedicationScheduleID { get; set; }
        public int PatientMedicationID { get; set; }
        public int Frequency { get; set; }

        public List<Days> ScheduleDays { get; set; }

        public PatientMedication PatientMedication { get; set; }
    }
}
