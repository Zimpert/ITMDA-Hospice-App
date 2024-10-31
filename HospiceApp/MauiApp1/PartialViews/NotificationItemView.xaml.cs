using MauiApp1.PartialViews;
using Microsoft.Maui.Controls;

namespace MauiApp1.PartialViews
{
    public partial class NotificationItemView : ContentView
    {
        public NotificationItemView()
        {
            InitializeComponent();
        }

        public string Title
        {
            get => TitleLabel.Text;
            set => TitleLabel.Text = value;
        }

        public string Description
        {
            get => DescriptionLabel.Text;
            set => DescriptionLabel.Text = value;
        }

        public ImageSource Icon
        {
            get => IconImage.Source;
            set => IconImage.Source = value;
        }
    }
}
