namespace Application.Abstractions;

public interface IEmailService
{
    Task SendEmailAsync(string from, string to, string toAddress, string subject, string body,
        CancellationToken ct = default);
}
