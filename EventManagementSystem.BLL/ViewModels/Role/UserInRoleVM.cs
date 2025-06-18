namespace EventManagementSystem.BLL.ViewModels.Role
{
    public record UserInRoleVM
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime? JoinedDate { get; set; }
    }
}
