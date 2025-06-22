using AutoMapper;
using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.User;
using EventManagementSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.BLL.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserDetailsVM> AddAsync(UserCreateVM viewModel)
        {
            var user = _mapper.Map<AppUser>(viewModel);
            var result = await _userManager.CreateAsync(user, viewModel.Password);
            if (!result.Succeeded)
            {
                return null;
            }
            var userDetailsVM = _mapper.Map<UserDetailsVM>(user);
            userDetailsVM.Role = (await _userManager.GetRolesAsync(user)).ToList();
            return userDetailsVM;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return false;
            }
            await _userManager.DeleteAsync(user);
            return true;
        }

        public async Task<IEnumerable<UserListVM>> GetAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userListVMs = new List<UserListVM>();

            foreach (var user in users)
            {
                var userVm = _mapper.Map<UserListVM>(user);
                var roles = await _userManager.GetRolesAsync(user);
                userVm.Role = roles.ToList();
                userListVMs.Add(userVm);
            }
            return userListVMs;
        }

        public async Task<UserDetailsVM?> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return null;
            }
            var userDetailsVM = _mapper.Map<UserDetailsVM>(user);
            userDetailsVM.Role = (await _userManager.GetRolesAsync(user)).ToList();
            return userDetailsVM;
        }

        public async Task<UserUpdateVM?> GetUpdateByIdAsync(string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id.ToString());
            if (user == null)
            {
                return null;
            }
            var userUpdateVM = _mapper.Map<UserUpdateVM>(user);
            return userUpdateVM;
        }

        public async Task<IEnumerable<UserListVM>> SearchUserAsync(string searchTerm)
        {
            var users = await _userManager.Users
                .Where(u => u.Name.Contains(searchTerm) || u.Surname.Contains(searchTerm) || u.Email.Contains(searchTerm))
                .ToListAsync();

            var userListVMs = new List<UserListVM>();
            foreach (var user in users)
            {
                var userVm = _mapper.Map<UserListVM>(user);
                var roles = await _userManager.GetRolesAsync(user);
                userVm.Role = roles.ToList();
                userListVMs.Add(userVm);
            }
            return userListVMs;
        }

        public async Task<UserDetailsVM> UpdateAsync(UserUpdateVM viewModel)
        {
            var user = await _userManager.FindByIdAsync(viewModel.Id.ToString());
            if (user == null)
            {
                return null;
                
            }

            _mapper.Map(viewModel, user);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return null;
            }

            var userVM = _mapper.Map<UserDetailsVM>(user);
            userVM.Role = (await _userManager.GetRolesAsync(user)).ToList();
            return userVM;
        }
    }
}