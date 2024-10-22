using MauiApp1.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    public class Logs
    {
        public string LogID { get; set; }
        public LogActionEnum LogAction { get; set; }
        public DateTime LogDate { get; set; }
        public string CaregiverID { get; set; }
        public string PatientID { get; set; }

    }
}
