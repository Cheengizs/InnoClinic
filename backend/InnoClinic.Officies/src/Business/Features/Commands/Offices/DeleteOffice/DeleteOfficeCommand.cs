using MediatR;
using Shared.Results;

namespace Business.Features.Commands.Officies.DeleteOffice;

public record DeleteOfficeCommand(Guid Id) : IRequest<Result>;
