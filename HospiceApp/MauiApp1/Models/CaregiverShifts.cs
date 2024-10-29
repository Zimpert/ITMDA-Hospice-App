using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp1.Models.Enums;
using MauiApp1.Models.PatientModels;

namespace MauiApp1.Models
{
    public class CaregiverShifts
    {
        public DateTime ShiftStart { get; set; }
        public DateTime ShiftEnd { get; set; }
        public string PatientName { get; set; }
        public string PatientSurname { get; set; }
        public string PatientAddress { get; set; }
    }
}
