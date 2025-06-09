using EventManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementSystem.DAL.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {

            builder.Property(au => au.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(au => au.Surname)
                .IsRequired()
                .HasMaxLength(100);

            builder.Ignore(au => au.Fullname);

            builder.Property(au => au.ImageUrl)
                .HasMaxLength(500);

            builder.Property(au => au.DateOfBirth)
                .IsRequired();

            builder.Property(au => au.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(au => au.IsActive)
                .IsRequired();

            builder.Property(au => au.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(au => au.UpdatedAt)
                .IsRequired(false);

            builder.Property(au => au.LastLoginDate)
                .IsRequired(false);

            builder.HasOne(au => au.Person)
                   .WithOne(p => p.AppUser)
                   .HasForeignKey<Person>(p => p.AppUserId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(au => au.Organizer)
                   .WithOne(o => o.AppUser)
                   .HasForeignKey<Organizer>(o => o.AppUserId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}