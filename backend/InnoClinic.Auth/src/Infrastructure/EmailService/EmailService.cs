using Application.Abstractions;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit.Text;

namespace Infrastructure.EmailService;

public class EmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;

    public EmailService(IOptions<EmailOptions> emailOptions)
    {
        _emailOptions = emailOptions.Value;
    }

    public async Task SendEmailAsync(string from, string to, string toAddress, string subject,
        string body, CancellationToken ct = default)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(from, _emailOptions.FromEmail));
        message.To.Add(new MailboxAddress(to, toAddress));
        message.Subject = subject;

        message.Body = new TextPart(TextFormat.Html) { Text = body };

        using var client = new SmtpClient();

        await client.ConnectAsync(_emailOptions.SmtpServer, _emailOptions.Port, SecureSocketOptions.None, ct);

        if (!string.IsNullOrWhiteSpace(_emailOptions.UserName))
        {
            await client.AuthenticateAsync(_emailOptions.UserName, _emailOptions.Password, ct);
        }

        await client.SendAsync(message, ct);

        await client.DisconnectAsync(true, ct);
    }
}
