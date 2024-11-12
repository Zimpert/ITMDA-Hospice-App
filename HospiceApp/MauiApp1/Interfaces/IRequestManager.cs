using MauiApp1.Models;
using static MauiApp1.Services.RequestManager;

namespace MauiApp1.Interfaces
{
    public interface IRequestManager
    {
        Task<User?> LoginAsync(string email, string password);

        Task<User?> GetUserDataAsync(string userID, string token);

        void Prelogin();

        Task<List<CaregiverShifts?>> GetCaregiverShiftsAsync(string userID, string token);

        Task<Dictionary<string, MedData?>> GetPatientMedicationsAsync(string token);
    }
}
