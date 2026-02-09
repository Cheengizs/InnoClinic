using Application.Dto.Account;
using Domain.Shared;
using MediatR;

namespace Application.Features.Accounts.Commands.Login;

public record LoginUserCommand(string Email, string Password) : IRequest<Result<LoginUserResponse>>;
