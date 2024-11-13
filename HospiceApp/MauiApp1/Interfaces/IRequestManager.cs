using MauiApp1.Models;
using static MauiApp1.Services.RequestManager;

namespace MauiApp1.Interfaces
{
    public interface IRequestManager
    {
        Task<User?> LoginAsync(string email, string password);

        // user info
        Task<User?> GetUserDataAsync(string userID, string token);


        Task Prelogin();

        Task<List<CaregiverShifts?>> GetCaregiverShiftsAsync(string userID, string token);

        Task<Dictionary<string, MedData?>> GetPatientMedicationsAsync(string token);

        Task<List<Medication>> GetPatientMedsONLY(string token, string targetID);

        Task<List<TaskM>> GetPatientTasks(string id);

        Task AddPatientTasks(string DateDue, string Description, string Token, string targetID);

        Task TaskLog(string token, string TaskID);

        Task MedLog(string token, string PatientMedicationID);
    }
}
