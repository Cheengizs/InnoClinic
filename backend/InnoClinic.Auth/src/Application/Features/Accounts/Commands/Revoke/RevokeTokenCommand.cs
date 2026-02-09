using Domain.Shared;
using MediatR;

namespace Application.Features.Accounts.Commands.Revoke;

public record RevokeTokenCommand(string RefreshToken) : IRequest<Result>;