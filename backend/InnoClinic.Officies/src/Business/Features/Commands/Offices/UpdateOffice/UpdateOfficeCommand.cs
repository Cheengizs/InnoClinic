using InnoClinic.Officies.Shared.Results;
using MediatR;

namespace Business.Features.Commands.Offices.UpdateOffice;

public record UpdateOfficeCommand(
    Guid Id,
    string City,
    string Street,
    string HouseNumber,
    string OfficeNumber,
    string PhotoUri,
    string RegistryPhoneNumber
) : IRequest<Result>;
