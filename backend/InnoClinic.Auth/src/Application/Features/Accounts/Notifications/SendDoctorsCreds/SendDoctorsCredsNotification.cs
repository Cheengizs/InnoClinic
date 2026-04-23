using MediatR;

namespace Application.Features.Accounts.Notifications.SendDoctorsCreds;

public record SendDoctorsCredsNotification(string Email, string Password) : INotification;
