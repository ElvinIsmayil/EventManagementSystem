namespace EventManagementSystem.BLL.ViewModels.Event
{
    public record EventDetailsVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public string OrganizerName { get; set; } = string.Empty;
        public string EventTypeName { get; set; } = string.Empty;
        public List<string> EventPhotosUrls { get; set; } = new List<string>();
        public List<string> Feedbacks { get; set; } = new List<string>();
    }
}
