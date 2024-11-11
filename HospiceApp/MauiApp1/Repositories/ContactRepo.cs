using System.Collections.ObjectModel;
using System.Linq;
using MauiApp1.Interfaces;
using MauiApp1.Models;

namespace MauiApp1.Repositories
{
    public class ContactRepo : IContactRepo
    {
        public ObservableCollection<ContactItem> ContactItemList { get; set; } = new ObservableCollection<ContactItem>();

        public List<Medication>? GetMedsByPatientID(string patientID)
        {
            var contact = ContactItemList.FirstOrDefault(c => c.PatientID == patientID);
            return contact?.Medication;
        }
    }
}
