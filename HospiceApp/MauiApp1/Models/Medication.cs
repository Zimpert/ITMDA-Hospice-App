using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace MauiApp1.Models
{
    public partial class Medication : ObservableObject
    {
        [ObservableProperty]
        [JsonPropertyName("Day")]
         string day;

        [ObservableProperty]
        [JsonPropertyName("Description")]
         string description;

        [ObservableProperty]
        [JsonPropertyName("Dosage")]
         string dosage;

        [ObservableProperty]
        [JsonPropertyName("EndDate")]
         string endDate;

        [ObservableProperty]
        [JsonPropertyName("Frequency")]
         string frequency;

        [ObservableProperty]
        [JsonPropertyName("Interactions")]
         string interactions;

        [ObservableProperty]
        [JsonPropertyName("MedicationName")]
         string medicationName;

        [ObservableProperty]
        [JsonPropertyName("StartDate")]
         string startDate;
    }
}
