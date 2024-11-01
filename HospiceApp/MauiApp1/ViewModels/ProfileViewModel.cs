using MauiApp1.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using MauiApp1.Interfaces;

namespace MauiApp1.ViewModels
{
    
public partial class ProfileViewModel : ObservableObject
    {
        private readonly IRequestManager _requestManager;

        // Backing field for the User property to store the user data
        [ObservableProperty]
        private User user;

        public ProfileViewModel(IRequestManager requestManager)
        {
            _requestManager = requestManager;

            // Initialize the User property to avoid null reference issues
            User = new User();
        }


        public async Task OnAppearingAsync()
        {
            var userID = await SecureStorage.GetAsync("UserID");
            var token = await SecureStorage.GetAsync("Token");

            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(token))
            {
                Console.WriteLine("User ID or token is missing.");
                return;
            }
            else
            {
                User userData = await _requestManager.GetUserDataAsync(userID, token);
                if (userData != null)
                {
                    User = userData;
                }
            }
        }
    }
}
