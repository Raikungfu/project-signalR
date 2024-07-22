using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using System.Dynamic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UserChatManagement;
using UserChatManagement.Controllers;
using UserChatManagement.Models;
using static EmailService;
using MessageBox = System.Windows.MessageBox;
using MessageBoxButton = System.Windows.MessageBoxButton;
using MessageBoxIcon = System.Windows.MessageBoxImage;

namespace SalesWPFApp
{
    public partial class WindowLogin : Window
    {
        private readonly ApplicationUserDAO _userDAO;
        private readonly RoomDAO _roomDAO;
        private readonly EmailSettings _emailSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HubConnection _hubConnection;

        public WindowLogin(ApplicationUserDAO userDAO, IOptions<EmailSettings> emailSettings, UserManager<ApplicationUser> userManager, HubConnection hubConnection, RoomDAO roomDAO)
        {
            InitializeComponent();
            _userDAO = userDAO;
            _emailSettings = emailSettings.Value;
            _userManager = userManager;
            _hubConnection = hubConnection;
            _roomDAO = roomDAO;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = txtId.Text;
            string password = txtPw.Password;
            try
            {
                var result = await _userDAO.LoginAdmin(username, password);
                if (result.SignInResult.Succeeded)
                {
                    await _hubConnection.InvokeAsync("AdminConnect", result.UserName);
                    var mainWindow = new MainWindow(this, _userDAO, _userManager, result.FullName, result.UserName, result.AvatarUrl, _hubConnection, _roomDAO, _emailSettings);
                    mainWindow.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Login Failed! ID/Password wasn't correct!!!", "ERROR", MessageBoxButton.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void BtnResetPassword_Click(object sender, RoutedEventArgs e)
        {
            var resetPasswordWindow = new ResetPasswordWindow(_userDAO, _emailSettings);
            resetPasswordWindow.ShowDialog();
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


        private void txtPwPlaceholder_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPwPlaceholder.Visibility = Visibility.Hidden;
            txtPw.Visibility = Visibility.Visible;
            txtPw.Focus();
        }

        private void txtPw_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPw.Password))
            {
                txtPw.Visibility = Visibility.Hidden;
                txtPwPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void txtPw_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPw.Password))
            {
                txtPwPlaceholder.Visibility = Visibility.Hidden;
            }
            else
            {
                txtPwPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void TxtId_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtId.Text == "Enter your account...")
            {
                txtId.Text = "";
                txtId.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void TxtId_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                txtId.Text = "Enter your account...";
                txtId.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }
    }
}
