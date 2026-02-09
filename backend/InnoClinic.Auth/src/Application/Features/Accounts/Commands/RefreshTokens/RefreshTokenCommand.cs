using Application.Dto.Account;
using Domain.Shared;
using MediatR;

namespace Application.Features.Accounts.Commands.RefreshTokens;

public record RefreshTokenCommand(string AccessToken, string RefreshToken) 
    : IRequest<Result<LoginUserResponse>>;