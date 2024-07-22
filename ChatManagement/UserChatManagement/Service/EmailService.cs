using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

public class EmailService
{
    public class EmailSettings
    {
        public string EmailID { get; set; }
        public string AppPassword { get; set; }
    }

    public static async Task SendEmailAsync(EmailSettings emailSettings, string emailTo, string subject, string message)
    {

        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Admin", emailSettings.EmailID));
        emailMessage.To.Add(new MailboxAddress("", emailTo));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("plain") { Text = message };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(emailSettings.EmailID, emailSettings.AppPassword);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }


    public static async Task SendEmailAsync(EmailSettings emailSettings, List<string> emailTo, string subject, string message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Admin", emailSettings.EmailID));

        foreach (var email in emailTo)
        {
            emailMessage.To.Add(new MailboxAddress("", email));
        }

        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("plain") { Text = message };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(emailSettings.EmailID, emailSettings.AppPassword);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}
