using Application.Abstractions;
using Application.Dto.Account;
using Domain.Models;
using Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Accounts.Commands.Login;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginUserResponse>>
{
    private readonly IAuthDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IHashProvider _hashProvider;
    private readonly IJwtProvider _jwtProvider;

    public LoginUserCommandHandler(IAuthDbContext context, IPasswordHasher passwordHasher, IJwtProvider jwtProvider, IHashProvider hashProvider)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _hashProvider = hashProvider;
    }

    public async Task<Result<LoginUserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

        if (account == null || !_passwordHasher.Verify(request.Password, account.PasswordHash))
        {
            return Result<LoginUserResponse>.Failure("Invalid email or password", ErrorType.Unauthorized);
        }

        var refreshTokens = await _context.RefreshTokens
            .Where(x => x.AccountId == account.Id && x.RevokedAt == null && x.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        refreshTokens.ForEach(x => x.Revoke("new login session"));

        var accessToken = _jwtProvider.GenerateJwtToken(account);
        var rawRefreshToken = _jwtProvider.GenerateRefreshToken();

        var refreshTokenHash = _hashProvider.GenerateHash(rawRefreshToken);

        var refreshTokenEntity = new RefreshToken(
            accountId: account.Id,
            tokenHash: refreshTokenHash,
            expiresAt: DateTime.UtcNow.AddDays(7)
        );

        await _context.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<LoginUserResponse>.Success(new(accessToken, rawRefreshToken));
    }
}
