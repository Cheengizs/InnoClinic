using DataAccess.Email;
using DataAccess.UsersService;
using MediatR;

namespace Business.Features.Notifications;

public class NotifyAdminsHandler : INotificationHandler<OfficeCreatedNotification>
{
    private readonly IEmailService _emailService;
    private readonly IUsersService _usersService;
    
    public NotifyAdminsHandler(IEmailService emailService, IUsersService usersService, IMediator mediator)
    {
        _emailService = emailService;
        _usersService = usersService;
    }

    public async Task Handle(OfficeCreatedNotification notification, CancellationToken ct)
    {
        var adminEmails = await _usersService.GetAllAdminsEmails();

        var subject = "New office!";
        var body = $@"
            <h1>New office in a system</h1>
            <p><strong>Name:</strong> {notification.OfficeName}</p>
            <p><strong>City:</strong> {notification.City}</p>
            <hr/>
            <p>Please, check the data in admin panel</p>";

        foreach (var email in adminEmails)
        {
            await _emailService.SendEmailAsync(email, subject, body, ct);
        }
    }
}
