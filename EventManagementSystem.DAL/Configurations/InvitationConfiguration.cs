using EventManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementSystem.DAL.Configurations
{
    public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
    {
        public void Configure(EntityTypeBuilder<Invitation> builder)
        {
            builder.ToTable("Invitations");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.SentAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(i => i.InvitationCode)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(i => i.InvitationCode).IsUnique();

            builder.Property(i => i.Status)
                .IsRequired()
                .HasConversion<string>();


            builder.Property(i => i.EventId).IsRequired();
            builder.Property(i => i.PersonId).IsRequired();

            builder.HasOne(i => i.Event)
                .WithMany(e => e.Invitations)
                .HasForeignKey(i => i.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(i => i.Person)
                .WithMany(p => p.Invitations)
                .HasForeignKey(i => i.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.Participation)
                   .WithOne(p => p.Invitation)
                   .HasForeignKey<Participation>(p => p.InvitationId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
