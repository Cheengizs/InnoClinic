using Application.Abstractions;
using Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Accounts.Queries.GetAdminsEmails;

public class GetAdminEmailsQueryHandler : IRequestHandler<GetAdminEmailsQuery, Result<List<string>>>
{
    private readonly IAuthDbContext _context;

    public GetAdminEmailsQueryHandler(IAuthDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<string>>> Handle(GetAdminEmailsQuery request, CancellationToken cancellationToken)
    {
        var adminEmails = await _context.Accounts.Where(x => x.Role == AccountRole.Admin).Select(x => x.Email)
            .ToListAsync(cancellationToken);
        return Result<List<string>>.Success(adminEmails);
    }
}
