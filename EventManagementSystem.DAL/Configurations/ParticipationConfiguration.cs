using EventManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementSystem.DAL.Configurations
{
    public class ParticipationConfiguration : IEntityTypeConfiguration<Participation>
    {
        public void Configure(EntityTypeBuilder<Participation> builder)
        {
            builder.ToTable("Participations");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.ParticipationDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.CheckInDate)
                .IsRequired();

            builder.Property(p => p.SeatNumber)
                .IsRequired();

            builder.Property(p => p.ConfirmationCode)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(p => p.ConfirmationCode).IsUnique();

            builder.Property(p => p.IsConfirmed)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(p => p.InvitationId)
                .IsRequired();

            builder.HasIndex(p => p.InvitationId).IsUnique(); // Uncomment for strict one-to-one
        }
    }
}
