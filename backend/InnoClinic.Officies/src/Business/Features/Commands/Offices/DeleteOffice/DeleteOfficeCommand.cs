using InnoClinic.Officies.Shared.Results;
using MediatR;

namespace Business.Features.Commands.Offices.DeleteOffice;

public record DeleteOfficeCommand(Guid Id) : IRequest<Result>;
