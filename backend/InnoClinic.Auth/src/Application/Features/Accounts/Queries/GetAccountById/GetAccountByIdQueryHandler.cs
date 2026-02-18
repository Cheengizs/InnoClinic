using Application.Abstractions;
using Application.Dto.Account;
using Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Accounts.Queries.GetAccountById;

public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, Result<AccountResponse>>
{
    private readonly IAuthDbContext _context;
    
    public GetAccountByIdQueryHandler(IAuthDbContext context)
    {
        _context = context;
    }

    public async Task<Result<AccountResponse>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var acc = await _context.Accounts
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(acc => new AccountResponse( 
                acc.Id,
                acc.Email,
                acc.Role,
                acc.PhoneNumber,
                acc.CreatedAt
            ))
            .FirstOrDefaultAsync(cancellationToken);
        
        if (acc is null)
        {
            return Result<AccountResponse>.Failure("Account not found", ErrorType.NotFound);
        }
        
        return Result<AccountResponse>.Success(acc);
    }
}
