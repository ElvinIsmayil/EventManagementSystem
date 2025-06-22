using AutoMapper;
using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.Person;
using EventManagementSystem.DAL.Entities;
using EventManagementSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.BLL.Services.Implementations
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public PersonService(IPersonRepository personRepository, IMapper mapper)
        {
            _personRepository = personRepository;
            _mapper = mapper;
        }

        public async Task<PersonDetailsVM> AddPersonAsync(PersonCreateVM personCreateVM)
        {
            if (personCreateVM == null)
            {
                throw new ArgumentNullException(nameof(personCreateVM), "Person creation data cannot be null.");
            }
            var person = _mapper.Map<Person>(personCreateVM);  
            var addedPerson = await _personRepository.AddAsync(person);
            if (addedPerson == null)
            {
                throw new InvalidOperationException("Failed to add person.");
            }
            return _mapper.Map<PersonDetailsVM>(addedPerson);

        }

        public async Task<bool> DeletePersonAsync(int personId)
        {
            var person = await _personRepository.GetByIdAsync(personId);
            if (person == null)
            {
                throw new KeyNotFoundException($"Person with ID {personId} not found.");
            }
            var result = await _personRepository.DeleteAsync(personId);
            if (!result)
            {
                throw new InvalidOperationException($"Failed to delete person with ID {personId}.");
            }
            return true;

        }

        public async Task<IEnumerable<PersonListVM>> GetAllPeopleAsync()
        {
            var people = await _personRepository.GetAllAsync();
            if (people == null || !people.Any())
            {
                throw new InvalidOperationException("No people found.");
            }
            var personListVMs = _mapper.Map<IEnumerable<PersonListVM>>(people);
            return personListVMs;
        }

        public async Task<PersonDetailsVM?> GetPersonByAppUserIdAsync(string appUserId)
        {
            if (string.IsNullOrEmpty(appUserId))
            {
                throw new ArgumentException("AppUserId cannot be null or empty.", nameof(appUserId));
            }
            var person = await _personRepository.GetPersonByAppUserIdAsync(appUserId);
            if (person == null)
            {
                throw new KeyNotFoundException($"Person with AppUserId {appUserId} not found.");
            }

            return _mapper.Map<PersonDetailsVM>(person);

        }

        public Task<PersonDetailsVM?> GetPersonByIdAsync(int personId)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetPersonEmailAsync(int personId)
        {
            throw new NotImplementedException();
        }

        public async Task<string?> GetPersonFullNameAsync(int personId)
        {
            var person = await _personRepository.GetPersonFullNameAsync(personId);

            // 2. Check if the person was found
            if (person == null)
            {
                return null; // Or throw an ArgumentException if a non-existent ID is considered an error
            }

            // 3. Return the Fullname from the AppUser (assuming AppUser has a Fullname property)
            return person;
        }

        public async Task<int?> GetPersonIdByAppUserIdAsync(string appUserId)
        {
            var person = await _personRepository.GetPersonByAppUserIdAsync(appUserId);
            if (person == null)
            {
                return null; // Person not found
            }
            return person.Id; // Return the PersonId


        }
    }
}
