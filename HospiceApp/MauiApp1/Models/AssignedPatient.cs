using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp1.Models.PatientModels;

namespace MauiApp1.Models
{
    public class AssignedPatient
    {
        public string AssignmentID { get; set; }
        public string CaregiverID { get; set; }
        public User Caregiver { get; set; }
        public string PatientID { get; set; }
        public User Patient { get; set; }


    }
}
