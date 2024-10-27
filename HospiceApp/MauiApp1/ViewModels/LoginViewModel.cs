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

        }

        [ObservableProperty]
        string loginText;

        [ObservableProperty]
        string passwordText;

        [RelayCommand]
        public async Task Login()
        {
            Debug.WriteLine("Login method called."); // Add this line to verify the method is called
            //var loginResult = await _requestManager.LoginAsync(loginText, passwordText);
            var loginResult = new Patient
            {
                UserID = "1211",
                Role = "Patient",
                Name = "John",
                Surname = "Doe",
                PhoneNo = "123456789",
                Email = "pete@gg.com",
                Address = "1234 Main St",
                PatientID = "211"
            };

            if (loginResult != null)//loginResult != null // Replace true with the condition to check if the login was successful
            {
                Debug.WriteLine("Login successful."); // Add this line to verify the login was successful

                if (loginResult is Patient patient)
                {
                    var navigationParams = new Patient
                    {
                        UserID = patient.UserID,
                        Role = patient.Role,
                        Name = patient.Name,
                        Surname = patient.Surname,
                        PhoneNo = patient.PhoneNo,
                        Email = patient.Email,
                        Address = patient.Address,
                        Token = patient.Token,
                        PatientID = patient.PatientID,

                    };
                    // Navigate to the appropriate page for patients
                    await Shell.Current.GoToAsync("//ProfilePage", true, new Dictionary<string, object>
                    {
                        { "User", navigationParams }
                    });
                }
                //else if (loginResult is Caregiver caregiver)
                //{
                //    var navigationParams = new Caregiver
                //    {
                //        UserID = caregiver.UserID,
                //        Role = caregiver.Role,
                //        Name = caregiver.Name,
                //        Surname = caregiver.Surname,
                //        PhoneNo = caregiver.PhoneNo,
                //        Email = caregiver.Email,
                //        Address = caregiver.Address,
                //        Token = caregiver.Token,
                //        CaregiverID = caregiver.CaregiverID
                //    };
                //    // Navigate to the appropriate page for caregivers
                //    await Shell.Current.GoToAsync("//ProfilePage", true, new Dictionary<string, object>
                //    {
                //        { "User", navigationParams }
                //    });
                //}
                else
                {
                    // Handle general user logic if needed( can become admin or familyMember)
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
                    await Shell.Current.GoToAsync("//ProfilePage", true, new Dictionary<string, object>
                    {
                        { "User", navigationParams }
                    });
                }
            }
            else
            {
                Debug.WriteLine("Login failed."); // Add this line to verify the login failed
            }
        }
    }
}
