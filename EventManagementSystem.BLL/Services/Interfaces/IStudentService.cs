using EventManagementSystem.BLL.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.BLL.Services.Interfaces
{
    public interface IStudentService
    {
        public Task<IEnumerable<UserDetailsVM>> GetUnverifiedStudents();
        public Task<bool> ApproveAsync(string studentId);   
        public Task<bool> DeleteAsync(string studentId);
        public Task<UserListVM> GetByIdAsync(string studentId);


    }
}
