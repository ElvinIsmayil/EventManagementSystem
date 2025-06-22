using EventManagementSystem.BLL.ViewModels.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.BLL.Services.Interfaces
{
    public interface IPersonService
    {
        Task<IEnumerable<PersonListVM>> GetAllPeopleAsync();
        Task<PersonDetailsVM?> GetPersonByIdAsync(int personId);
        Task<PersonDetailsVM> AddPersonAsync(PersonCreateVM personCreateVM);
        Task<bool> DeletePersonAsync(int personId);
        Task<int?> GetPersonIdByAppUserIdAsync(string appUserId);
        Task<PersonDetailsVM?> GetPersonByAppUserIdAsync(string appUserId);
        Task<string?> GetPersonFullNameAsync(int personId);
        Task<string?> GetPersonEmailAsync(int personId);



    }
}
