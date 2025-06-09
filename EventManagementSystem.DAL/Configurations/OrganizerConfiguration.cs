using EventManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementSystem.DAL.Configurations
{
    public class OrganizerConfiguration : IEntityTypeConfiguration<Organizer>
    {
        public void Configure(EntityTypeBuilder<Organizer> builder)
        {
            builder.Property(o => o.PublicEmail)
                   .HasMaxLength(256);

            builder.Property(o => o.PublicPhoneNumber)
                   .HasMaxLength(20);

            builder.Property(o => o.PublicWebsite)
                   .HasMaxLength(500);

            builder.Property(o => o.AverageRating)
                   .IsRequired();

            builder.Property(o => o.IsApproved)
                   .IsRequired();

            builder.HasOne(o => o.AppUser)
                   .WithOne(au => au.Organizer)
                   .HasForeignKey<Organizer>(o => o.AppUserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.OrganizedEvents)
                   .WithOne(e => e.Organizer)
                   .HasForeignKey(e => e.OrganizerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}