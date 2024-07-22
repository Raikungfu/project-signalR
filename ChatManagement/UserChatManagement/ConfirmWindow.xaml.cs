using System.Windows;

namespace UserChatManagement
{
    public partial class ConfirmationWindow : Window
    {
        public string ConfirmationTitle { get; set; }

        public ConfirmationWindow(string title, string message)
        {
            InitializeComponent();
            DataContext = this;
            ConfirmationTitle = title;
            MessageTextBlock.Text = message;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
