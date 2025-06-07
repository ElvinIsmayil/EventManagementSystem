using EventManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementSystem.DAL.Configurations
{
    public class EventTypeConfiguration : IEntityTypeConfiguration<EventType>
    {
        public void Configure(EntityTypeBuilder<EventType> builder)
        {
            builder.ToTable("EventTypes");

            builder.HasKey(et => et.Id);

            builder.Property(et => et.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(et => et.Description)
                .HasMaxLength(500);


            builder.HasMany(et => et.Events)
                .WithOne(e => e.EventType)
                .HasForeignKey(e => e.EventTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
