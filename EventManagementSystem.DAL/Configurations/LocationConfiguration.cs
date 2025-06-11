using EventManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementSystem.DAL.Configurations
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Locations");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(l => l.Description)
                .HasMaxLength(1000);

            builder.Property(l => l.Address)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(l => l.Capacity)
                .IsRequired();


            builder.HasMany(l => l.locationPhotos)
                .WithOne(lp => lp.Location)
                .HasForeignKey(lp => lp.LocationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(l => l.Events)
                .WithOne(e => e.Location)
                .HasForeignKey(e => e.LocationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
