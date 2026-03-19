using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace DataAccess.Email;

public class EmailService(IOptions<EmailOptions> emailOptions) : IEmailService
{
    private readonly EmailOptions _options = emailOptions.Value;

    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken ct = default)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_options.UserName, _options.FromEmail));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        
        message.Body = new TextPart(TextFormat.Html) { Text = body };

        using var smtp = new SmtpClient();
        
        await smtp.ConnectAsync(_options.SmtpServer, _options.Port, SecureSocketOptions.None, ct);
        
        if (!string.IsNullOrEmpty(_options.UserName) && !string.IsNullOrEmpty(_options.Password))
        {
            await smtp.AuthenticateAsync(_options.UserName, _options.Password, ct);
        }
        
        await smtp.SendAsync(message, ct);
        await smtp.DisconnectAsync(true, ct);
    }
}
