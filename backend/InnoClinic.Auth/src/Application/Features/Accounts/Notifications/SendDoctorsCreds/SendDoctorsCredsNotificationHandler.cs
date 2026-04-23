using Application.Abstractions;
using MediatR;

namespace Application.Features.Accounts.Notifications.SendDoctorsCreds;

public class SendDoctorsCredsNotificationHandler : INotificationHandler<SendDoctorsCredsNotification>
{
    private readonly IEmailService _emailService;

    public SendDoctorsCredsNotificationHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Handle(SendDoctorsCredsNotification notification, CancellationToken cancellationToken)
    {
        var text = $"your creds have been created\n email: {notification.Email}\n password: {notification.Password}";

        await _emailService.SendEmailAsync("InnoClinic", "new doctor", notification.Email, "creds for new account",
            text, cancellationToken);
        
    }
}
