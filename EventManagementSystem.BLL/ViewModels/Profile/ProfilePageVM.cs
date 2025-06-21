namespace EventManagementSystem.BLL.ViewModels.Profile
{
    public record ProfilePageVM
    {
        public ProfileDetailsVM ProfileDetails { get; set; }
        public ProfileUpdateVM ProfileUpdate { get; set; }
        public ChangePasswordVM ChangePassword { get; set; }
        public ProfilePictureUploadVM ProfilePictureUpload { get; set; }

        // You might add properties for managing which section is active if you use tabs/accordions on the UI
        public string ActiveTab { get; set; } = "details"; // e.g., "details", "update", "password", "picture"
    }
}
