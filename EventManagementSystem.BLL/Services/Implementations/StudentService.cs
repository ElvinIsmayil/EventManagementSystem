using AutoMapper;
using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.User;
using EventManagementSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace EventManagementSystem.BLL.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public StudentService(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDetailsVM>> GetUnverifiedStudents()
        {
            var users = await _userManager.GetUsersInRoleAsync("Student");
            var unverifiedStudents = users.Where(u => !u.IsStudentApproved);
            return unverifiedStudents.Select(u => new UserDetailsVM
            {
                Id = u.Id,
                Name = u.UserName,
                Email = u.Email,
                IsStudent = u.IsStudentApproved,
                FileUrl = "https://localhost:7170/" + u.StudentImageUrl,
            }).ToList();
        }

        public async Task<bool> ApproveAsync(string studentId)
        {
            var student = await _userManager.FindByIdAsync(studentId);
            if (student == null || student.IsStudentApproved)
            {
                return false; // Student not found or already approved
            }
            student.IsStudentApproved = true;
            var result = await _userManager.UpdateAsync(student);
            return result.Succeeded;
        }

        public async Task<bool> DeleteAsync(string studentId)
        {
            var student = await _userManager.FindByIdAsync(studentId);
            if (student == null)
            {
                return false; // Student not found
            }
            var result = await _userManager.DeleteAsync(student);
            return result.Succeeded;
        }

        public async Task<UserListVM?> GetByIdAsync(string studentId)
        {
            var user = await _userManager.FindByIdAsync(studentId);
            var userListVM = _mapper.Map<UserListVM>(user);
            if (userListVM == null)
            {
                return null; // User not found
            }

            return userListVM;
        }
    }
}
