using Domain.Models;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DbConfiguring;

public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("accounts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(Constants.MaxEmailLength);
        
        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.PasswordHash)
            .IsRequired();

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(Constants.MaxNumberLength); 

        builder.Property(x => x.Role)
            .HasConversion<string>()
            .HasMaxLength(Constants.MaxRoleNameLength)
            .IsRequired();

        builder.Metadata.FindNavigation(nameof(Account.RefreshTokens))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(x => x.RefreshTokens)
            .WithOne()
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
