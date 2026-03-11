using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace DataAccess.ModelsConfiguring;

public class OfficeEntityConfiguration : IEntityTypeConfiguration<Office>
{
    public void Configure(EntityTypeBuilder<Office> builder)
    {
        builder.ToTable("offices");
        
        builder.
            HasKey(x => x.Id);

        builder.Property(x => x.City)
            .HasMaxLength(DbConfigConstants.MaxCityLength)
            .IsRequired();
        
        builder.Property(x => x.Street)
            .HasMaxLength(DbConfigConstants.MaxStreetLength)
            .IsRequired();
        
        builder.Property(x => x.HouseNumber)
            .HasMaxLength(DbConfigConstants.MaxHouseNumberLength)
            .IsRequired();
        
        builder.Property(x => x.OfficeNumber)
            .HasMaxLength(DbConfigConstants.MaxOfficeNumberLength)
            .IsRequired();

        builder.Property(x => x.RegistryPhoneNumber)
            .HasMaxLength(DbConfigConstants.MaxRegistryPhoneNumberLength)
            .IsRequired();

        builder.HasOne(x => x.Photo)
            .WithOne(x => x.Office)
            .HasForeignKey<Office>(x => x.PhotoId);
        
        builder.Property(x => x.IsActive)
            .IsRequired();
    }
}
