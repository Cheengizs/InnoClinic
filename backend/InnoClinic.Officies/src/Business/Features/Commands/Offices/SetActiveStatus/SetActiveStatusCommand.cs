using MediatR;
using Shared.Results;

namespace Business.Features.Commands.Officies.SetActiveStatus;

public record SetActiveStatusCommand(Guid Id, bool IsActive) : IRequest<Result>;
