using Application.Abstractions;
using Domain.Models;
using Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Accounts.Commands.RegisterAccount;

public class RegisterAccountCommandHandler : IRequestHandler<RegisterAccountCommand, Result<Guid>>
{
    private readonly IAuthDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterAccountCommandHandler(IAuthDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<Guid>> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == request.Email, cancellationToken);

        if (account is not null)
        {
            return Result<Guid>.Failure("Account with this email already exists", ErrorType.Conflict);
        }
        
        var newAccount = new Account(
            email: request.Email,
            passwordHash: _passwordHasher.Hash(request.Password),
            role: request.Role,
            phoneNumber: request.PhoneNumber
            );
        
        await _context.Accounts.AddAsync(newAccount, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Result<Guid>.Success(newAccount.Id);
    }
}
