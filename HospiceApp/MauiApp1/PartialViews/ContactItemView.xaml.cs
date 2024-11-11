using Microsoft.Maui.Controls;
using MauiApp1.PartialViews;
using MauiApp1.Models;

namespace MauiApp1.PartialViews
{
    public partial class ContactItemView : ContentView
    {
        public ContactItemView()
        {
            InitializeComponent();
        }

        public string Name
        {
            get => NameLabel.Text;
            set => NameLabel.Text = value;
        }

        private async void TakeToNewPage(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///PatientMiddlePage");
        }

        //public string Details
        //{
        //    get => DetailsLabel.Text;
        //    set => DetailsLabel.Text = value;
        //}

    }
}
