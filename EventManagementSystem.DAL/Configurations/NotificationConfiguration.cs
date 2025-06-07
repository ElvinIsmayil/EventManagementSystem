using EventManagementSystem.DAL.Entities;
using EventManagementSystem.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementSystem.DAL.Configurations
{

    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.Message)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(n => n.SentAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(n => n.RecipientEmail)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(n => n.Type)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(n => n.IsRead)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(n => n.Status)
                .IsRequired(false)
                .HasDefaultValue(NotificationStatus.Pending);

            builder.Property(n => n.ErrorMessage)
                .HasMaxLength(1000);

            builder.Property(n => n.ScheduledAt)
                .IsRequired(false);

            builder.Property(n => n.EventId)
                .IsRequired(false);

            builder.Property(n => n.PersonId)
                .IsRequired();

            builder.HasOne(n => n.Event)
                   .WithMany()
                   .HasForeignKey(n => n.EventId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(n => n.Person)
                   .WithMany(p => p.Notifications)
                   .HasForeignKey(n => n.PersonId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
