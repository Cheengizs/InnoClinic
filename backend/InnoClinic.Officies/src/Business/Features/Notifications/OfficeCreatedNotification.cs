using MediatR;

namespace Business.Features.Notifications;

public record OfficeCreatedNotification(
    Guid OfficeId, 
    string OfficeName, 
    string City) : INotification;
