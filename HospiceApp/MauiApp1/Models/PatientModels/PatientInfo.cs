using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiApp1.Models.PatientModels
{
    public class PatientInfo
    {

        [JsonPropertyName("PatientName")]
        public string PatientName { get; set; }

        [JsonPropertyName("PatientSurname")]
        public string PatientSurname { get; set; }

    }
}
