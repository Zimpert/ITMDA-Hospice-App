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
            Debug.WriteLine("Login method called."); // Add this line to verify the method is called
            var loginResult = await _requestManager.LoginAsync(LoginText, PasswordText);


            //uncomment the following line to test the login page
            loginResult = new User
            {
                UserID = "wqw",
                Role = "Patient",
                Name = "John",
                Surname = "Doe",
                PhoneNo = "123456789",
                Email = "dasdsadsad",
                Address = "dasdsadsad",
                Token = "dasdsadsad"
            };

            if (loginResult != null)
            {
                Debug.WriteLine("Login successful."); // Add this line to verify the login was successful

                var navigationParams = new User
                {
                    UserID = loginResult.UserID,
                    Role = loginResult.Role,
                    Name = loginResult.Name,
                    Surname = loginResult.Surname,
                    PhoneNo = loginResult.PhoneNo,
                    Email = loginResult.Email,
                    Address = loginResult.Address,
                    Token = loginResult.Token
                };

                if (Shell.Current != null)
                {
                    await Shell.Current.GoToAsync("///ProfilePage", true, new Dictionary<string, object>
                    {
                        { "User", navigationParams }
                    });
                }
                else
                {
                    Debug.WriteLine("Shell.Current is null.");
                }


            }
            else
            {
                Debug.WriteLine("Login failed."); // Add this line to verify the login failed
            }
        }
    }
}
