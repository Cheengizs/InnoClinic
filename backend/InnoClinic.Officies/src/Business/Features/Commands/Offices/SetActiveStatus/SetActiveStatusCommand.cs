using InnoClinic.Officies.Shared.Results;
using MediatR;

namespace Business.Features.Commands.Offices.SetActiveStatus;

public record SetActiveStatusCommand(Guid Id, bool IsActive) : IRequest<Result>;
