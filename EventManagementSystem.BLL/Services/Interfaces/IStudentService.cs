using EventManagementSystem.BLL.ViewModels.User;

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
