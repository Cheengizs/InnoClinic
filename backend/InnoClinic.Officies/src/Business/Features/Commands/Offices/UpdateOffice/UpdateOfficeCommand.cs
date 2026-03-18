using MediatR;
using Shared.Results;

namespace Business.Features.Commands.Offices.UpdateOffice;

public record UpdateOfficeCommand(
    Guid Id,
    string City,
    string Street,
    string HouseNumber,
    string OfficeNumber,
    Guid? PhotoId,
    string RegistryPhoneNumber
    ) : IRequest<Result>;
