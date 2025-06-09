using EventManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementSystem.DAL.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.Property(p => p.AppUserId)
                   .IsRequired();

            builder.HasOne(p => p.AppUser)
                   .WithOne(au => au.Person)
                   .HasForeignKey<Person>(p => p.AppUserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Invitations)
                   .WithOne(i => i.Person)
                   .HasForeignKey(i => i.PersonId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Notifications)
                   .WithOne(n => n.Person)
                   .HasForeignKey(n => n.PersonId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Feedbacks)
                   .WithOne(f => f.Person)
                   .HasForeignKey(f => f.PersonId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}