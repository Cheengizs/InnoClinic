using Application.Abstractions;
using Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Accounts.Commands.Revoke;

public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, Result>
{
    private readonly IAuthDbContext _context;
    private readonly IHashProvider _hashProvider;

    public RevokeTokenCommandHandler(IAuthDbContext context, IHashProvider hashProvider)
    {
        _context = context;
        _hashProvider = hashProvider;
    }

    public async Task<Result> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var tokenHash = _hashProvider.GenerateHash(request.RefreshToken);

        var existingToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);

        if (existingToken is null)
        {
            return Result.Failure("Token not found", ErrorType.NotFound);
        }

        if (existingToken.IsActive) 
        {
            existingToken.Revoke("Manual logout");
            await _context.SaveChangesAsync(cancellationToken);
        }

        return Result.Success();
    }
}