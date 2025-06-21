using EventManagementSystem.DAL.Data;
using EventManagementSystem.DAL.Entities;
using EventManagementSystem.DAL.Repositories.Interfaces;

namespace EventManagementSystem.DAL.Repositories.Implementations
{
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        public PersonRepository(EventManagementSystemDbContext context) : base(context)
        {
        }

    }
}
