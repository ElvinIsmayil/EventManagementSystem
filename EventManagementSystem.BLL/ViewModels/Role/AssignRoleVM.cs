using EventManagementSystem.BLL.ViewModels.User;

namespace EventManagementSystem.BLL.ViewModels.Role
{
    public record AssignRoleVM
    {
        public string UserId { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public string RoleId { get; set; } = string.Empty;
        public List<UserListVM> Users { get; set; } = new List<UserListVM>();
        public List<RoleListVM> Roles { get; set; } = new List<RoleListVM>();
    }
}
