using MauiApp1.Models;
using MauiApp1.Models.PatientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.ViewModels
{
    public class CaregiverShiftViewModel
    {
        public DateTime ShiftStart { get; set; }
        public DateTime ShiftEnd { get; set; }
        public Days Day { get; set; }
        public Patient Patient { get; set; }

        //public User User { get; set; }
    }
}
