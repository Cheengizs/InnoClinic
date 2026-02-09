using Application.Abstractions;
using Application.Dto.Account;
using Domain.Models;
using Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Accounts.Commands.RefreshTokens;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<LoginUserResponse>>
{
    private readonly IAuthDbContext _context;
    private readonly IJwtProvider _jwtProvider;
    private readonly IHashProvider _hashProvider;

    public RefreshTokenCommandHandler(
        IAuthDbContext context,
        IJwtProvider jwtProvider,
        IHashProvider hashProvider)
    {
        _context = context;
        _jwtProvider = jwtProvider;
        _hashProvider = hashProvider;
    }

    public async Task<Result<LoginUserResponse>> Handle(RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var tokenHash = _hashProvider.GenerateHash(request.RefreshToken);

        var existingRefreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);

        if (existingRefreshToken is null)
        {
            return Result<LoginUserResponse>.Failure("Invalid refresh token", ErrorType.NotFound);
        }

        if (existingRefreshToken.IsRevoked)
        {
            return Result<LoginUserResponse>.Failure("Token has been revoked", ErrorType.Unauthorized);
        }

        if (existingRefreshToken.IsExpired)
        {
            return Result<LoginUserResponse>.Failure("Token expired", ErrorType.Unauthorized);
        }

        var account = await _context.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == existingRefreshToken.AccountId, cancellationToken);

        if (account is null)
        {
            return Result<LoginUserResponse>.Failure("Account not found", ErrorType.NotFound);
        }

        existingRefreshToken.Revoke("Refresh token rotation");

        var newAccessToken = _jwtProvider.GenerateJwtToken(account);
        var newRawRefreshToken = _jwtProvider.GenerateRefreshToken();

        var newRefreshTokenHash = _hashProvider.GenerateHash(newRawRefreshToken);

        var newRefreshTokenEntity = new RefreshToken(
            accountId: account.Id,
            tokenHash: newRefreshTokenHash,
            expiresAt: DateTime.UtcNow.AddDays(7)
        );

        await _context.RefreshTokens.AddAsync(newRefreshTokenEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<LoginUserResponse>.Success(new LoginUserResponse(newAccessToken, newRawRefreshToken));
    }
}