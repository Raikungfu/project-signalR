using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;
using UserChatManagement.Controllers;
using UserChatManagement.Models;

namespace UserChatManagement
{
    public partial class EditUserWindow : Window
    {
        private readonly ApplicationUserDAO _userDao;
        private readonly ApplicationUser _user;
        private readonly HubConnection _hubConnection;

        public EditUserWindow(ApplicationUser user, ApplicationUserDAO userDao, HubConnection hubConnection)
        {
            InitializeComponent();
            _hubConnection = hubConnection;
            _userDao = userDao;
            _user = user;

            UserNameTextBox.Text = _user.UserName;
            FullNameTextBox.Text = _user.FullName;
            EmailTextBox.Text = _user.Email;
            PhoneTextBox.Text = _user.PhoneNumber;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _user.FullName = FullNameTextBox.Text;
            _user.Email = EmailTextBox.Text;
            _user.PhoneNumber = PhoneTextBox.Text;

            var result = await _userDao.UpdateApplicationUser(_user);
            if (result.Succeeded)
            {
                await SendUpdateNotification();

                MessageBox.Show("User updated successfully.");
                this.Close();
            }
            else
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                MessageBox.Show($"Failed to update user! Errors: {errorMessage}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async Task SendUpdateNotification()
        {
            var messageParts = new List<string>();

            if (!string.IsNullOrEmpty(_user.FullName))
            {
                messageParts.Add($"Full Name - {_user.FullName}");
            }

            if (!string.IsNullOrEmpty(_user.Email))
            {
                messageParts.Add($"Email - {_user.Email}");
            }

            if (!string.IsNullOrEmpty(_user.PhoneNumber))
            {
                messageParts.Add($"Phone Number - {_user.PhoneNumber}");
            }

            var message = $"{_user.UserName} has been updated. " +
                          string.Join(", ", messageParts);

            try
            {
                await _hubConnection.SendAsync("SendNotification", message);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Could not connect to chat room: {ex.Message}");
            }
        }

    }
}
