
using System.Collections.Concurrent;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using static EmailService;
using Microsoft.Extensions.Options;

namespace UserChatManagement.Worker
{
    public class MailEventWorker : BackgroundService
    {
        private readonly ILogger<MailEventWorker> _logger;
        private readonly EmailService _emailService;
        private readonly EmailSettings _emailSettings;
        private readonly ConcurrentQueue<(string recipientEmail, string subject, string message)> _emailQueue;
        private readonly TimeSpan _delayTime;

        public MailEventWorker(
            ILogger<MailEventWorker> logger,
            IOptions<EmailSettings> emailSettings,
            TimeSpan delayTime)
        {
            _logger = logger;
            _emailSettings = emailSettings.Value;
            _emailQueue = new ConcurrentQueue<(string recipientEmail, string subject, string message)>();
            _delayTime = delayTime;
        }

        public void QueueEmail(string recipientEmail, string subject, string message, TimeSpan delayTime)
        {
            _emailQueue.Enqueue((recipientEmail, subject, message, delayTime));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_emailQueue.TryDequeue(out var email))
                {
                    try
                    {
                        await Task.Delay(email.delayTime, stoppingToken);
                        await _emailService.SendEmailAsync(_emailSettings, email.recipientEmail, email.subject, email.message);
                        _logger.LogInformation($"Email sent to {email.recipientEmail} with subject: {email.subject}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Failed to send email to {email.recipientEmail}. Error: {ex.Message}");
                    }
                }
                else
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
            }
        }
    }

}