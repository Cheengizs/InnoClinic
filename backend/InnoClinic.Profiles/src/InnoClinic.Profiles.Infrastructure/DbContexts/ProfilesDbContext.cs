using InnoClinic.Profiles.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Infrastructure.DbContexts;

public class ProfilesDbContext : DbContext
{
    public ProfilesDbContext(DbContextOptions<ProfilesDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProfilesDbContext).Assembly);
    }

    public DbSet<Doctor> Doctors { get; set; }
}
