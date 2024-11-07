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

        //public string Details
        //{
        //    get => DetailsLabel.Text;
        //    set => DetailsLabel.Text = value;
        //}

    }
}
