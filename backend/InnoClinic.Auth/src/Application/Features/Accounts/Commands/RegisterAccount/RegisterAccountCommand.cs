using Domain.Shared;
using MediatR;

namespace Application.Features.Accounts.Commands.RegisterAccount;

public record RegisterAccountCommand(
    string Email, 
    string Password, 
    string? PhoneNumber, 
    AccountRole Role) : IRequest<Result<Guid>>;
