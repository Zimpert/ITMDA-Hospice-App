using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    
    
    public class CareGiverManager
    {
        private RequestManager _requestManager;
        public CareGiverManager(RequestManager requestManager) 
        { 
            _requestManager = requestManager;
        }

        public void GetShifts(string CaregiverID)
        {
            
            // return the data from their schedule from the database 
            // need to get shifts by the caregiver ID to group it

            // whats the return data ?

            // needs to return the joined data of 
            // CaregiverShift, Shift and Patient Data
        }

        public void GenerateQRCode()
        {
            // code to generate the QR code
        }



    }


}
