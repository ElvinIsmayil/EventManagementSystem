namespace EventManagementSystem.BLL.ViewModels.Role
{
    public record RoleDetailsVM
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int UserCount { get; set; }

        public List<UserInRoleVM> Users { get; set; } = new List<UserInRoleVM>();
        public List<UserInRoleVM> AvailableUsers { get; set; } = new List<UserInRoleVM>();
        public string SelectedUserId { get; set; } = null!;

    }
}
