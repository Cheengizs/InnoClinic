using DataAccess.Models;
using DataAccess.ModelsConfiguring;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DbContexts;

public class OfficesDbContext : DbContext
{
    public OfficesDbContext(DbContextOptions<OfficesDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OfficesDbContext).Assembly);
    }

    public DbSet<Office> Offices => Set<Office>();
}
