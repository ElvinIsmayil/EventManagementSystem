using EventManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementSystem.DAL.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("Users");

            builder.Property(au => au.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(au => au.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(au => au.UpdatedAt)
                .IsRequired(false);

            builder.HasOne(au => au.Person)
                   .WithOne(p => p.AppUser)
                   .HasForeignKey<Person>(p => p.AppUserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(au => au.Organizer)
                   .WithOne(o => o.AppUser)
                   .HasForeignKey<Organizer>(o => o.AppUserId)
                   .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
