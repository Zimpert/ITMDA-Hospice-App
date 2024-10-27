using MauiApp1.ViewModels;

namespace MauiApp1
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage(ProfileViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is ProfileViewModel vm)
            {
                vm.OnAppearing(); // Call the method on ViewModel
            }
        }
    }
}
