using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var emailSettings = _configuration.GetSection("MailSettings");

        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(emailSettings["SenderName"], emailSettings["SenderEmail"]));
        emailMessage.To.Add(new MailboxAddress("", toEmail)); // To Email (form's email, or a fixed recipient)
        emailMessage.Subject = subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = body };
        emailMessage.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync(emailSettings["SmtpServer"],
                                          int.Parse(emailSettings["SmtpPort"]),
                                          SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(emailSettings["Username"],
                                               emailSettings["Password"]);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new Exception("Email sending failed", ex);
            }
        }
    }
}
