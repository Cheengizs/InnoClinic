using Domain.Shared;
using MediatR;

namespace Application.Features.Accounts.Commands.InsertUser;

public record InsertUserCommand(
    Guid Id,
    string Email, 
    string Password, 
    string? PhoneNumber, 
    AccountRole Role) : IRequest<Result>;
