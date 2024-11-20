using MauiApp1.Interfaces;
using MauiApp1.ViewModels;

namespace MauiApp1.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly IRequestManager _reqMan;
        public LoginPage(LoginViewModel viewModel, IRequestManager reqMan)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _reqMan = reqMan;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Your custom logic here
            Console.WriteLine("Page is now appearing!");
            await _reqMan.Prelogin();
        }

    }
}

