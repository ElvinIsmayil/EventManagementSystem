using EventManagementSystem.BLL.ViewModels.User;

namespace EventManagementSystem.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDetailsVM?> GetByIdAsync(string id);
        Task<IEnumerable<UserListVM>> GetAllAsync();
        Task<UserDetailsVM> AddAsync(UserCreateVM viewModel);
        Task<bool> DeleteAsync(string id);
        Task<UserDetailsVM> UpdateAsync(UserUpdateVM viewModel);
        Task<UserUpdateVM?> GetUpdateByIdAsync(string id);
        Task<IEnumerable<UserListVM>> SearchUserAsync(string searchTerm);

    }
}
