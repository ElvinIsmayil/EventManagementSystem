namespace EventManagementSystem.BLL.ViewModels.Role
{
    public record UserInRoleVM
    {
        public string Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public DateTime? JoinedDate { get; set; }
    }
}
