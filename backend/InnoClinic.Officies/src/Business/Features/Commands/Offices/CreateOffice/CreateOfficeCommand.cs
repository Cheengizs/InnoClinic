using MediatR;
using Shared.Results;

namespace Business.Features.Commands.Officies.CreateOffice;

public record CreateOfficeCommand(
    string City,
    string Street,
    string HouseNumber,
    string OfficeNumber,
    Guid? PhotoId,
    string RegistryPhoneNumber)
    : IRequest<Result<Guid>>;
