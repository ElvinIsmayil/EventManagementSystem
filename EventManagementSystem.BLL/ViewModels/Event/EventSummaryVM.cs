namespace EventManagementSystem.BLL.ViewModels.Event
{
    public record EventSummaryVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? MainImageUrl { get; set; }
    }
}
