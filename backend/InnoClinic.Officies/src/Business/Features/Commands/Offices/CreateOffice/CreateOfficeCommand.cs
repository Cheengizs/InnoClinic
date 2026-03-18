using MediatR;
using Shared.Results;

namespace Business.Features.Commands.Offices.CreateOffice;

public record CreateOfficeCommand(
    string City,
    string Street,
    string HouseNumber,
    string OfficeNumber,
    string PhotoUri,
    string RegistryPhoneNumber)
    : IRequest<Result<Guid>>;
