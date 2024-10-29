using Microsoft.Maui.Controls;

namespace MauiApp1.PartialViews
{

    public partial class TaskItemView : ContentView
    {
        public TaskItemView()
        {
            InitializeComponent();
        }

        public string TaskTitle
        {
            get => TaskTitleLabel.Text;
            set => TaskTitleLabel.Text = value;
        }

        public string TaskDescription
        {
            get => TaskDescriptionLabel.Text;
            set => TaskDescriptionLabel.Text = value;
        }

        public bool IsCompleted
        {
            get => TaskCompletionCheckBox.IsChecked;
            set => TaskCompletionCheckBox.IsChecked = value;
        }
    }
}