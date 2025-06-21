using EventManagementSystem.BLL.ViewModels.Event;

namespace EventManagementSystem.BLL.ViewModels.EventType
{
    public record EventTypeDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<EventSummaryVM> eventSummary { get; set; } = null!;
    }
}