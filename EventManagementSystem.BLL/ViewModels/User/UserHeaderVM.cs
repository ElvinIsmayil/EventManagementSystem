namespace EventManagementSystem.BLL.ViewModels.User
{
    public class UserHeaderVM
    {
        public string Id { get; set; }
        public string ImageUrl { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        // ProjectsCount property has been removed
    }
}