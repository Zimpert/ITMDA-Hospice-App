using System.Collections.ObjectModel;
using MauiApp1.Models;

namespace MauiApp1.Interfaces
{
    /// <summary>
    /// Interface for managing contact repository.
    /// </summary>
    public interface IContactRepo
    {
        /// <summary>
        /// Gets or sets the list of contact items.
        /// </summary>
        ObservableCollection<ContactItem> ContactItemList { get; set; }

        /// <summary>
        /// Retrieves a list of medications for a patient by their ID.
        /// </summary>
        /// <param name="patientID">The ID of the patient.</param>
        /// <returns>A list of medications for the specified patient.</returns>
        List<Medication?> GetMedsByPatientID(string patientID);
    }
}
