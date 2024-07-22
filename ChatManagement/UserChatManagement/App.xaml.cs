using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesWPFApp;
using System;
using System.Threading.Tasks;
using System.Windows;
using UserChatManagement.Controllers;
using UserChatManagement.Models;
using Microsoft.Extensions.Options;

using static EmailService;
using UserChatManagement.Worker;

namespace UserChatManagement
{
    public partial class App : Application
    {
        private HubConnection _hubConnection;
        private IServiceProvider _serviceProvider;
        public static IServiceProvider ServiceProvider { get; private set; }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();


            _serviceProvider = serviceCollection.BuildServiceProvider();

            InitializeSignalR();

            var windowLogin = new WindowLogin(
                _serviceProvider.GetRequiredService<ApplicationUserDAO>(),
                _serviceProvider.GetRequiredService<IOptions<EmailSettings>>(),
                _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>(),
                _hubConnection,
                _serviceProvider.GetRequiredService<RoomDAO>()
            );

            windowLogin.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            services.AddLogging();
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings").Bind);
            var emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();
            services.AddSingleton(emailSettings);
            services.AddTransient<EmailService>();
            services.AddHostedService<MailEventWorker>();

            services.AddScoped<ApplicationUserDAO>();
            services.AddScoped<RoomDAO>();
            services.AddScoped<MainWindow>();
            services.AddScoped<WindowLogin>();
            services.AddSingleton<UserManager<ApplicationUser>>();
        }

        private async void InitializeSignalR()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/chatHub")
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<string>("ReceiveNewUserNotification", (userName) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"{userName} has joined!");
                });
            });

            _hubConnection.On<string>("ReceiveHelpRequest", (userName) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"{userName} needs help!");
                });
            });

            _hubConnection.On<string, bool>("updateUserStatus", (userName, isOnline) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    string status = isOnline ? "is online" : "has disconnected";
                    MessageBox.Show($"{userName} {status}");
                });
            });

            _hubConnection.Closed += async (error) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("Disconnected from the server. Attempting to reconnect...");
                });

                while (_hubConnection.State == HubConnectionState.Disconnected)
                {
                    try
                    {
                        await Task.Delay(new Random().Next(0, 5) * 1000);
                        await _hubConnection.StartAsync();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show("Reconnected to the server.");
                        });
                    }
                    catch (Exception ex)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show($"Reconnect failed: {ex.Message}");
                        });
                    }
                }
            };

            try
            {
                await _hubConnection.StartAsync();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("Connected to the server.");
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to the server: {ex.Message}");
            }
        }
    }
}
