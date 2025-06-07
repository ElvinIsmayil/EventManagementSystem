using EventManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementSystem.DAL.Configurations
{
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.ToTable("Feedbacks");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Rating)
                .IsRequired();

            builder.Property(f => f.Comment)
                .HasMaxLength(1000);

            builder.Property(f => f.SubmittedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(f => f.PersonId).IsRequired();
            builder.Property(f => f.EventId).IsRequired();

            builder.HasOne(f => f.Person)
                .WithMany(p => p.Feedbacks)
                .HasForeignKey(f => f.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.Event)
                .WithMany(e => e.Feedbacks)
                .HasForeignKey(f => f.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
