using MediatR;
using Shared.Results;

namespace Business.Features.Commands.Offices.DeleteOffice;

public record DeleteOfficeCommand(Guid Id) : IRequest<Result>;
