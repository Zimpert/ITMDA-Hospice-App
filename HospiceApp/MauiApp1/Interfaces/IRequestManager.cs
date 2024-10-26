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
    public  interface IRequestManager
    {
        Task<Patient?> PostPatientAsync(string method);

         Task<User?> Login(string method, string email, string password);


    }
}
