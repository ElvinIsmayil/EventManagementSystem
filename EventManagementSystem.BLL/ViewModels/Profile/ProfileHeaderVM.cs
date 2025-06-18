namespace EventManagementSystem.BLL.ViewModels.Profile
{
    public record ProfileHeaderVM
    {
        public string Id { get; set; } = null!;
        public string? ImageUrl { get; set; } = null!;
        public string Fullname { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public bool IsLoggedIn => !string.IsNullOrEmpty(Id);
    }


}