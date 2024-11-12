using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp1.Interfaces;
using MauiApp1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.ViewModels
{
    public partial class PatientDetailsViewModel : ObservableObject, IQueryAttributable
    {
        private IRequestManager _requestManager;


        public Action MedsLoaded;
        public string targetID;

        [ObservableProperty]
        private User? user;

        public PatientDetailsViewModel(IRequestManager requestManager)
        {
            _requestManager = requestManager;
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            targetID = query["PatientID"].ToString();

            GetPatientDetails();
        }

        public async void GetPatientDetails()
        {
            try
            {
                var token = await SecureStorage.GetAsync("Token");

                if (string.IsNullOrEmpty(targetID) || string.IsNullOrEmpty(token))
                {
                    Debug.WriteLine("User ID or token is missing.");
                    return;
                }
                else
                {
                    User? userData = await _requestManager.GetUserDataAsync(targetID, token);
                    if (userData != null)
                    {
                        User = userData;
                    }
                    else
                    {
                        Debug.WriteLine("Failed to retrieve user data.");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
