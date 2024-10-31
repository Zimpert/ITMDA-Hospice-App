using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Interfaces;
using MauiApp1.Models;
using MauiApp1.Models.PatientModels;
using System.Diagnostics;


namespace MauiApp1.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IRequestManager _requestManager;

        public LoginViewModel(IRequestManager requestManager)
        {
            _requestManager = requestManager;
            loginText = string.Empty; // Initialize loginText
            passwordText = string.Empty; // Initialize passwordText
        }

        [ObservableProperty]
        string loginText;

        [ObservableProperty]
        string passwordText;

        [RelayCommand]
        public async Task Login()
        {
            Console.WriteLine("Login method called.");
            var loginResult = await _requestManager.LoginAsync(LoginText, PasswordText);

            if (loginResult != null)
            {
                Console.WriteLine("Login successful."); 
                var navigationParams = new User
                {
                    UserID = loginResult.UserID,
                    Token = loginResult.Token
                };

                if (Shell.Current != null)
                {
                    await SecureStorage.SetAsync("Token", loginResult.Token);
                    await SecureStorage.SetAsync("UserID", loginResult.UserID);
                    await Shell.Current.GoToAsync("///HomePage"); // Navigate to the HomePage
                }
                else
                {
                    Console.WriteLine("Shell.Current is null.");
                }
            }
            else
            {
                Console.WriteLine("Login failed.");
                
            }
        }
    }
}
