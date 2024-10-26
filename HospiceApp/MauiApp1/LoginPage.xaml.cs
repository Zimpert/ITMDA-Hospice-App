using MauiApp1.Interfaces;
using MauiApp1.ViewModels;

namespace MauiApp1;

public partial class LoginPage : ContentPage
{
    private readonly IRequestManager _requestManager;

    // Parameterless constructor for the framework
    public LoginPage()
    {
        InitializeComponent();
    }

    // Constructor with dependency injection
    public LoginPage(IRequestManager requestManager)
        : this() // Call the parameterless constructor
    {
        _requestManager = requestManager;
        BindingContext = new LoginViewModel(_requestManager);
    }
}
