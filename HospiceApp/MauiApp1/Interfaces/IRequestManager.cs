using MauiApp1.Models;
using MauiApp1.Models.PatientModels;
using MauiApp1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Interfaces
{
    public interface IRequestManager
    {
        Task<User?> LoginAsync(string email, string password);

        Task<User?> GetUserDataAsync(string userID, string token);

        Task<bool> ValidateToken();


    }
}
