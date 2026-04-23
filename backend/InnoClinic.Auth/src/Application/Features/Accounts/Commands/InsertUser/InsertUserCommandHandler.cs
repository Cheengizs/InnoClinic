using Application.Abstractions;
using Application.Features.Accounts.Notifications.SendDoctorsCreds;
using Domain.Models;
using Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Accounts.Commands.InsertUser;

public class InsertUserCommandHandler : IRequestHandler<InsertUserCommand, Result>
{
    private readonly IAuthDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMediator _mediator;
    
    public InsertUserCommandHandler(IPasswordHasher passwordHasher, IAuthDbContext context, IMediator mediator)
    {
        _passwordHasher = passwordHasher;
        _context = context;
        _mediator = mediator;
    }

    public async Task<Result> Handle(InsertUserCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == request.Email, cancellationToken);

        if (account is not null)
        {
            return Result.Failure("Account with this email already exists", ErrorType.Conflict);
        }

        var doctorCreatedNotification = new SendDoctorsCredsNotification(request.Email, request.Password);
        await _mediator.Publish(doctorCreatedNotification);
        
        var newAccount = new Account(
            email: request.Email,
            passwordHash: _passwordHasher.Hash(request.Password),
            role: request.Role,
            phoneNumber: request.PhoneNumber
        );
        
        await _context.Accounts.AddAsync(newAccount, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
