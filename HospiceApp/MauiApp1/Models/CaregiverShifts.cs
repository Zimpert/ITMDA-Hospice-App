using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MauiApp1.Models.Enums;
using MauiApp1.Models.PatientModels;

namespace MauiApp1.Models
{
    public class CaregiverShifts
    {
        public string ShiftStart { get; set; }
        public string ShiftEnd { get; set; }

        [JsonIgnore]
        public string Time { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
    }
}
