using EventManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementSystem.DAL.Configurations
{
    public class OrganizerConfiguration : IEntityTypeConfiguration<Organizer>
    {
        public void Configure(EntityTypeBuilder<Organizer> builder)
        {
            builder.ToTable("Organizers");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.Surname)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(o => o.Email).IsUnique();

            builder.Property(o => o.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(o => o.IsApproved)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(o => o.AppUserId)
                .IsRequired()
                .HasMaxLength(450);

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
