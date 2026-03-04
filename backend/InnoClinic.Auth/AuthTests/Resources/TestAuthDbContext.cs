using Application.Abstractions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthTests.Resources;

public class TestAuthDbContext : DbContext, IAuthDbContext
{
    public TestAuthDbContext(DbContextOptions<TestAuthDbContext> options)
        : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}
