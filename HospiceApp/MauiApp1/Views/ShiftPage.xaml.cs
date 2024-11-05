using MauiApp1.ViewModels;

namespace MauiApp1.Views
{
	public partial class ShiftPage : ContentPage
	{
        public ShiftPage(CaregiverShiftViewModel csvm)
        {
            InitializeComponent();
            BindingContext = csvm;
            Task.Run(async () => await csvm.GetData());
        }

    }
}