using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.ApplicationServices;
using SalesWPFApp;
using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UserChatManagement.Controllers;
using UserChatManagement.Models;
using UserChatManagement.ViewModels;
using UserChatManagement.Worker;
using static EmailService;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace UserChatManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApplicationUserDAO _userDao;
        private readonly RoomDAO _roomDao;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HubConnection _hubConnection;
        private string _adminUserName;
        private string currentSender;
        private string currentRoomName;
        private readonly EmailSettings _emailSettings;
        private readonly MailEventWorker _mailEventWorker;

        private WindowLogin windowLogin;
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(WindowLogin _windowLogin, ApplicationUserDAO userDao, UserManager<ApplicationUser> userManager, string adminFullName, string adminUserName, string adminAvatar, HubConnection hubConnection, RoomDAO roomDao, EmailSettings emailSettings)
        {
            InitializeComponent();
            _hubConnection = hubConnection;
            windowLogin = _windowLogin;
            _userDao = userDao;
            _userManager = userManager;
            AdminNameTextBlock.Text = adminFullName;
            string baseUrl = "https://localhost:5001";
            string avatarPath = $"{baseUrl}/avatars/{adminAvatar}";
            var imageBrush = (ImageBrush)AdminAvatarImage.Fill;
            imageBrush.ImageSource = new BitmapImage(new Uri(avatarPath, UriKind.RelativeOrAbsolute));
            _adminUserName = adminUserName;
            LoadUsers();
            _roomDao = roomDao;
            _emailSettings = emailSettings;
            _mailEventWorker = App.ServiceProvider.GetRequiredService<MailEventWorker>();

        }

        private async void LoadUsers()
        {
            var users = await _userDao.GetApplicationUsersAsync();
            UserListBox.ItemsSource = users;
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addUserWindow = new AddUserWindow(_userDao, _hubConnection);
            if (addUserWindow.ShowDialog() == true)
            {
                LoadUsers();
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserListBox.SelectedItem is ApplicationUser user)
            {
                var editUserWindow = new EditUserWindow(user, _userDao, _hubConnection);
                if (editUserWindow.ShowDialog() == true)
                {
                    LoadUsers();
                }
            }
        }

        private async void UserListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UserListBox.SelectedItem is ApplicationUser selectedUser)
            {
                if (UserDetailsPanel.Visibility == Visibility.Collapsed) UserDetailsPanel.Visibility = Visibility.Visible;

                if (ChatPanel.Visibility == Visibility.Visible) ChatPanel.Visibility = Visibility.Collapsed;
                FullNameTextBlock.Text = selectedUser.FullName;
                EmailTextBlock.Text = selectedUser.Email;
                PhoneNumberTextBlock.Text = selectedUser.PhoneNumber;
                UserNameTextBlock.Text = selectedUser.UserName;

                if (!string.IsNullOrEmpty(selectedUser.Avatar))
                {
                    string baseUrl = "https://localhost:5001";
                    string avatarPath = $"{baseUrl}/avatars/{selectedUser.Avatar}";
                    AvatarImage.Source = new BitmapImage(new Uri(avatarPath, UriKind.Absolute));
                }
                else
                {
                    AvatarImage.Source = new BitmapImage(new Uri("/Assets/LogoWhiteBack.png", UriKind.Relative));
                }

                if (selectedUser.EmailConfirmed)
                {
                    EmailConfirmedIcon.Source = new BitmapImage(new Uri("/Assets/ConfirmedIcon.png", UriKind.Relative));
                }
                else
                {
                    EmailConfirmedIcon.Source = new BitmapImage(new Uri("/Assets/UnconfirmedIcon.png", UriKind.Relative));
                }

                var roles = await _userManager.GetRolesAsync(selectedUser);
                RolesTextBlock.Text = string.Join(", ", roles);
            }
        }

        private async void searchQuery_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchTerm = searchQuery.Text;
            var users = await _userDao.SearchApplicationUsers(searchTerm);
            UserListBox.ItemsSource = users;
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void LogoutButton_Click(object sender, MouseButtonEventArgs e)
        {
            windowLogin.Show();
            System.Windows.MessageBox.Show("You have logged out.");
            this.Close();
        }

        private void AdminAvatarImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LogoutPopup.IsOpen = !LogoutPopup.IsOpen;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (LogoutPopup.IsOpen)
            {
                LogoutPopup.IsOpen = false;
            }
        }

        private async void ChatButton_Click(object sender, RoutedEventArgs e)
        {
            MessageListBox.Items.Clear();
            var button = sender as System.Windows.Controls.Button;
            currentSender = button?.CommandParameter as string;

            if (string.IsNullOrEmpty(currentSender))
            {
                System.Windows.MessageBox.Show("User not found.");
                return;
            }

            if(UserDetailsPanel.Visibility == Visibility.Visible) UserDetailsPanel.Visibility = Visibility.Collapsed;
            if (ChatPanel.Visibility == Visibility.Collapsed) ChatPanel.Visibility = Visibility.Visible;

            var room = await _roomDao.StartChat(currentSender, _adminUserName);

            if (room == null)
            {
                System.Windows.MessageBox.Show("User or Admin not found.");
                return;
            }

            currentRoomName = room.Name;

            if (room.Messages != null)
            {
                foreach (var message in room.Messages)
                {
                    var messageViewModel = new MessageViewModel
                    {
                        Id = message.Id,
                        Content = message.Content,
                        Timestamp = message.Timestamp,
                        FromUserName = message.FromUser.UserName,
                        FromFullName = message.FromUser.FullName,
                        Room = room.Name,
                        Avatar = message.FromUser.Avatar
                    };

                    AddMessageToChat(messageViewModel);
                }
            }
            try
            {
                await _hubConnection.InvokeAsync("JoinRoom", room.Name, currentSender);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Could not connect to chat room: {ex.Message}");
            }
            InitializeChatHub();
        }

        private void InitializeChatHub()
        {
            _hubConnection.On<MessageViewModel>("newMessage", (message) =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (ChatPanel.Visibility == Visibility.Visible && message.Room == currentRoomName)
                    {
                        AddMessageToChat(message);
                    }
                });
            });
        }

        private void AddMessageToChat(MessageViewModel message)
        {
            var messageContainer = new DockPanel
            {
                LastChildFill = true,
                Margin = new Thickness(5, 2, 5, 2)
            };

            string baseUrl = "https://localhost:5001";
            string avatarPath = $"{baseUrl}/avatars/{message.Avatar}";
            var avatarImage = new Image
            {
                Source = new BitmapImage(new Uri(avatarPath, UriKind.RelativeOrAbsolute)),
                Width = 30,
                Height = 30,
                Margin = new Thickness(5)
            };

            var nameTextBlock = new TextBlock
            {
                Text = message.FromFullName,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(5, 0, 5, 0)
            };

            var textBlock = new TextBlock
            {
                Text = message.Content,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(10, 2, 10, 2),
                MaxWidth = 300
            };

            var timestampTextBlock = new TextBlock
            {
                Text = message.Timestamp.ToString("g"),
                FontStyle = FontStyles.Italic,
                FontSize = 12,
                Margin = new Thickness(5, 0, 5, 0)
            };

            var border = new Border
            {
                Background = message.FromUserName == _adminUserName ? new SolidColorBrush(Colors.LightGray) : new SolidColorBrush(Colors.LightBlue),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(5),
                Margin = new Thickness(5, 2, 5, 2)
            };

            var borderContainer = new StackPanel();
            borderContainer.Children.Add(nameTextBlock);
            borderContainer.Children.Add(textBlock);
            borderContainer.Children.Add(timestampTextBlock);

            border.Child = borderContainer;

            DockPanel.SetDock(avatarImage, Dock.Left);

            messageContainer.Children.Add(avatarImage);
            messageContainer.Children.Add(border);

            MessageListBox.Items.Add(messageContainer);
            MessageListBox.ScrollIntoView(messageContainer);
        }


        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string messageContent = ChatInputBox.Text;
            if (!string.IsNullOrWhiteSpace(messageContent))
            {
                var createdMessage = await _roomDao.SaveMessageAsync(currentRoomName, _adminUserName, messageContent);

                if (createdMessage != null)
                {
                    try
                    {
                        await _hubConnection.InvokeAsync("NewMessage", createdMessage);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show($"Could not connect to chat room: {ex.Message}");
                    }
                    ChatInputBox.Clear();
                }
                else
                {
                    System.Windows.MessageBox.Show("Failed to send message. Room or user not found.");
                }
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomePanel.Visibility = Visibility.Visible;
            EventPanel.Visibility = Visibility.Collapsed;
        }

        private void EventButton_Click(object sender, RoutedEventArgs e)
        {
            HomePanel.Visibility = Visibility.Collapsed;
            EventPanel.Visibility = Visibility.Visible;
        }

        private async Task SendEmailWorkerButton_Click(object sender, RoutedEventArgs e)
        {
            string subject = EventSubjectTextBox.Text;
            string message = EventMessageTextBox.Text;
            string delayTimeStr = EventDelayTimeTextBox.Text;
            TimeSpan delayTime = TimeSpan.TryParse(delayTimeStr, out var result) ? result : TimeSpan.FromMinutes(1); // Default to 1 minute if parsing fails

            var users = await _userDao.GetApplicationUsersAsync();

            foreach (var user in users)
            {
                _mailEventWorker.QueueEmail(user.Email, subject, message);
            }

            System.Windows.MessageBox.Show("Emails queued successfully!");
        }
    }
}
