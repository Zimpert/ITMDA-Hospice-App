using MauiApp1.Interfaces;
using MauiApp1.Services;
using MauiApp1.ViewModels;
using System.Diagnostics;

namespace MauiApp1.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly RequestManager _requestManager;
        public LoginPage(LoginViewModel viewModel, RequestManager requestManager)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _requestManager = requestManager;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            Debug.WriteLine("Part1 Start");
            await _requestManager.Prelogin();
        }


        //protected override async void OnAppearing()
        //{
        //    base.OnAppearing();
        //    Debug.WriteLine("AppShell appearing.");

        //    await _requestManager.Prelogin();
        //}

    }
}

