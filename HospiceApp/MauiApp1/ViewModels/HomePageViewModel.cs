using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.ViewModels
{
    public partial class HomePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string userRole; // Example: "Carer", "Admin", "Patient", etc.

        private readonly IRequestManager _requestManager;

        public HomePageViewModel(IRequestManager reqMan)
        {
            _requestManager = reqMan;
            _ = GetUserRole();
        }

        public async Task GetUserRole()
        {
            UserRole = await SecureStorage.GetAsync("Role");
        }

    }



}
