using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static EmailService;
using UserChatManagement.Controllers;

namespace UserChatManagement
{
    /// <summary>
    /// Interaction logic for ResetPasswordWindow.xaml
    /// </summary>
    // ResetPasswordWindow.xaml.cs
    public partial class ResetPasswordWindow : Window
    {
        private readonly ApplicationUserDAO _userDAO;
        private readonly EmailSettings _emailSettings;

        public ResetPasswordWindow()
        {
            InitializeComponent();
        }

        public ResetPasswordWindow(ApplicationUserDAO userDAO, EmailSettings emailSettings)
        {
            InitializeComponent();
            _userDAO = userDAO;
            _emailSettings = emailSettings;
        }

        private async void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            var email = txtEmail.Text;
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Please enter an email.");
                return;
            }

            var newPassword = GenerateRandomPassword();

            try
            {
                await _userDAO.UpdatePasswordAsync(email, newPassword);


                var subject = "Admin Account Password Change Notification";
                var message = $"Hello,\n\nYour new password for the admin account is: {newPassword}\n\nPlease change your password immediately after logging in.\n\nBest regards,\nSupport Team";

                await EmailService.SendEmailAsync(_emailSettings, email, subject, message);
                MessageBox.Show("Sent new password to your email.");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred: {ex.Message}");
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private string GenerateRandomPassword(int length = 8)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var random = new Random();
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(validChars.Length)];
            }
            return new string(chars);
        }
    }

}
