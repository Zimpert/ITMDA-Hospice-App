using MauiApp1.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

namespace MauiApp1.ViewModels
{
    [QueryProperty(nameof(User), "User")]
    public partial class ProfileViewModel : ObservableObject
    {
        [ObservableProperty]
        private User user;

        public ProfileViewModel()
        {
            User = new User();
        }

        public void OnAppearing()
        {
            // Perform any actions needed when the page appears
        }
    }
}
