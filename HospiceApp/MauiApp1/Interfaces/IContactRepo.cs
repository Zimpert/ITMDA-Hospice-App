using System.Collections.ObjectModel;
using MauiApp1.Models;

namespace MauiApp1.Interfaces
{
    public interface IContactRepo
    {
        ObservableCollection<ContactItem> ContactItemList { get; set; }
        List<Medication?> GetMedsByPatientID(string patientID);
    }
}
