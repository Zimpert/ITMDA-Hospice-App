using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.ViewModels
{
    public partial class QRGenerationViewModel : ObservableObject
    {

        [ObservableProperty]
        private string _barcodeValue;

        public QRGenerationViewModel()
        {
            InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            // Fetch the user ID from SecureStorage and set it as the default for BarcodeValue
            _barcodeValue = await SecureStorage.GetAsync("UserID") ?? "DefaultUserID";
        }


    }
}
