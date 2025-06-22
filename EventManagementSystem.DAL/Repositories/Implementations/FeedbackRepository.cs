using EventManagementSystem.DAL.Data;
using EventManagementSystem.DAL.Entities;
using EventManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.DAL.Repositories.Implementations
{
    public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(EventManagementSystemDbContext context) : base(context)
        {
        }
        public async Task<Feedback> GetFeedbackDetailsByIdAsync(int id)
        {
            // Assuming that the Feedback entity has navigation properties for Person and Event
            return await _dbSet
                .Where(f => f.Id == id)
                .Select(f => new Feedback
                {
                    Id = f.Id,
                    Rating = f.Rating,
                    Comment = f.Comment,
                    SubmittedAt = f.SubmittedAt,
                    PersonId = f.PersonId,
                    Person = new Person
                    {
                        Id = f.Person.Id,
                        AppUser = new AppUser
                        {
                            UserName = f.Person.AppUser.Name,
                            Email = f.Person.AppUser.Email
                        }
                    },
                    EventId = f.EventId,
                    Event = new Event
                    {
                        Id = f.Event.Id,
                        Title = f.Event.Title,
                        StartDate = f.Event.StartDate
                    }
                })
                .FirstOrDefaultAsync();

        }

        public Task<IEnumerable<Feedback>> GetFeedbacksWithPersonAndEventAsync()
        {
            // Assuming that the Feedback entity has navigation properties for Person and Event
            return _dbSet
                .Include(f => f.Person)
                    .ThenInclude(p => p.AppUser)
                .Include(f => f.Event)
                .Select(f => new Feedback
                {
                    Id = f.Id,
                    Rating = f.Rating,
                    Comment = f.Comment,
                    SubmittedAt = f.SubmittedAt,
                    PersonId = f.PersonId,
                    Person = new Person
                    {
                        Id = f.Person.Id,
                        AppUser = new AppUser
                        {
                            UserName = f.Person.AppUser.Name,
                            Email = f.Person.AppUser.Email
                        }
                    },
                    EventId = f.EventId,
                    Event = new Event
                    {
                        Id = f.Event.Id,
                        Title = f.Event.Title,
                        StartDate = f.Event.StartDate
                    }
                })
                .ToListAsync()
                .ContinueWith(task => task.Result.AsEnumerable());

        }
    }
}
