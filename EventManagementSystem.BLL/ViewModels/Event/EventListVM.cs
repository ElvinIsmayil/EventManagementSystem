namespace EventManagementSystem.BLL.ViewModels.Event
{
    public record EventListVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
        public string OrganizerName { get; set; } = string.Empty;
        public string EventTypeName { get; set; } = string.Empty;
        public string? MainPhotoUrl { get; set; } = string.Empty;
    }
}
