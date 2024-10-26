using MauiApp1.Classes;
using MauiApp1.Interfaces;
using MauiApp1.Services;
using MauiApp1.ViewModels;

namespace MauiApp1
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            //big fat penis
        }
    }

}

