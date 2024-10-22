using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models.PatientModels
{
    public class Patient
    {
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        public string PatientSurname { get; set; }
        public string PatientPhoneNo { get; set; }
        public string PatientEmail { get; set; }
        public string PatientAddress { get; set; }

    }
}
