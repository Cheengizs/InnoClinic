using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions;

public interface IAuthDbContext
{
    DbSet<Account> Accounts { get; set;  }
    DbSet<RefreshToken> RefreshTokens { get; set;  }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}