using MediatR;
using Shared.Results;

namespace Business.Features.Commands.Offices.SetActiveStatus;

public record SetActiveStatusCommand(Guid Id, bool IsActive) : IRequest<Result>;
