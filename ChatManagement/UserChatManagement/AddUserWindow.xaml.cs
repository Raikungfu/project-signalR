using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.VisualBasic.ApplicationServices;
using System.Windows;
using UserChatManagement.Controllers;
using UserChatManagement.Models;

namespace UserChatManagement
{
    public partial class AddUserWindow : Window
    {
        private readonly ApplicationUserDAO _userDao;
        private readonly HubConnection _hubConnection;

        public AddUserWindow(ApplicationUserDAO userDao, HubConnection hubConnection)
        {
            InitializeComponent();
            _userDao = userDao;

            _hubConnection = hubConnection;
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var user = new ApplicationUser
            {
                UserName = UserNameTextBox.Text,
                FullName = FullNameTextBox.Text,
                Email = EmailTextBox.Text,
                PhoneNumber = PhoneTextBox.Text,
            };
            var password = PasswordBox.Password;

            var result = await _userDao.AddApplicationUser(user, password);
            if (result.Succeeded)
            {
                try
                {
                    var messageParts = new List<string>();

                    if (!string.IsNullOrEmpty(user.FullName))
                    {
                        messageParts.Add($"Full Name - {user.FullName}");
                    }

                    if (!string.IsNullOrEmpty(user.Email))
                    {
                        messageParts.Add($"Email - {user.Email}");
                    }

                    if (!string.IsNullOrEmpty(user.PhoneNumber))
                    {
                        messageParts.Add($"Phone Number - {user.PhoneNumber}");
                    }

                    var message = $"{user.UserName} has been updated. " +
                                  string.Join(", ", messageParts);

                    await _hubConnection.SendAsync("SendNotification",
                        $"New user {user.UserName} has been added. " +
                        string.Join(", ", messageParts));

                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Could not connect to chat room: {ex.Message}");
                }
                MessageBox.Show("User added successfully.");
                this.Close();
            }
            else
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                MessageBox.Show($"Error adding user: {errorMessage}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
