using EventManagementSystem.DAL.Configurations;
using EventManagementSystem.DAL.Entities;
using EventManagementSystem.DAL.Entities.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventManagementSystem.DAL.Data
{
    public class EventManagementSystemDbContext : IdentityDbContext<AppUser>
    {
        public EventManagementSystemDbContext(DbContextOptions<EventManagementSystemDbContext> options)
        : base(options)
        {
        }
        public DbSet<Person> People { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Participation> Participations { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Organizer> Organizers { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<EventPhoto> EventPhotos { get; set; }
        public DbSet<LocationPhoto> LocationPhotos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventConfiguration).Assembly);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.FindProperty(nameof(BaseEntity.CreatedAt))?
                        .SetDefaultValueSql("GETUTCDATE()");

                    var isDeletedProperty = entityType.FindProperty(nameof(BaseEntity.IsDeleted));
                    if (isDeletedProperty != null)
                    {
                        isDeletedProperty.SetDefaultValue(false);
                        entityType.AddIndex(isDeletedProperty);
                    }

                    var parameter = Expression.Parameter(entityType.ClrType, "entity");
                    var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                    var filter = Expression.Lambda(Expression.Equal(property, Expression.Constant(false)), parameter);
                    entityType.SetQueryFilter(filter);
                }
            }

        }


    }
}
