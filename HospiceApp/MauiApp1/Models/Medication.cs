using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    public class Medication
    {

        [JsonPropertyName("Day")]
        public string Day { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("Dosage")]
        public string Dosage { get; set; }

        [JsonPropertyName("EndDate")]
        public string EndDate { get; set; }

        [JsonPropertyName("Frequency")]
        public string Frequency { get; set; }

        [JsonPropertyName("Interactions")]
        public string Interactions { get; set; }

        [JsonPropertyName("MedicationName")]
        public string MedicationName { get; set; }

        [JsonPropertyName("StartDate")]
        public string StartDate { get; set; }

    }
}
