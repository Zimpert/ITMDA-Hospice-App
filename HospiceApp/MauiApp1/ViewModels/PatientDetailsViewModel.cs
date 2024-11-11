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
                User? userData = await _requestManager.GetUserDataAsync(targetID);

                if (userData != null)
                {
                    User = userData;
                    
                }
                else
                {
                    Debug.WriteLine("Failed to retrieve user data.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }




    }
}
