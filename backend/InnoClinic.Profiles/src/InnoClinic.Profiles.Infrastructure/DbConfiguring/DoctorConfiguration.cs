using InnoClinic.Profiles.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnoClinic.Profiles.Infrastructure.DbConfiguring;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("doctors");
        
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasColumnName("id");

        builder.HasIndex(d => d.Email).IsUnique();
        builder.HasIndex(d => d.AccountId).IsUnique(); 
        builder.HasIndex(d => d.SpecializationId);     
        builder.HasIndex(d => d.OfficeId);             
        
        builder.Property(d => d.Email)
            .HasColumnName("email")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(d => d.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(d => d.MiddleName)
            .HasColumnName("middle_name")
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(d => d.PhotoUrl)
            .HasColumnName("photo_url")
            .HasMaxLength(500)
            .IsRequired(false); 

        builder.Property(d => d.DateOfBirth)
            .HasColumnName("date_of_birth")
            .IsRequired();

        builder.Property(d => d.AccountId)
            .HasColumnName("account_id")
            .IsRequired();

        builder.Property(d => d.SpecializationId)
            .HasColumnName("specialization_id")
            .IsRequired();

        builder.Property(d => d.OfficeId)
            .HasColumnName("office_id")
            .IsRequired();

        builder.Property(d => d.CareerStartYear)
            .HasColumnName("career_start_year")
            .IsRequired();

        builder.Property(d => d.Status)
            .HasColumnName("status")
            .IsRequired();
        
    }
}
