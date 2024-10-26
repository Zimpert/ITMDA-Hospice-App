using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp1.Models.Enums;
using MauiApp1.Models.PatientModels;

namespace MauiApp1.Models
{
    class CaregiverShifts
    {
        public string CaregiverShiftID { get; set; }
        public string CaregiverID { get; set; }
        public Caregiver Caregiver { get; set; }
        public string PatientID { get; set; }
        public Patient Patient { get; set; }
        public string ReminderID { get; set; }
        public Reminders Reminders { get; set; }
        
        public DateTime ShiftStart { get; set; }
        public DateTime ShiftEnd { get; set; }
    }
}
