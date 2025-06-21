using EventManagementSystem.BLL.ViewModels.Event;
using EventManagementSystem.BLL.ViewModels.EventType;

namespace EventManagementSystem.BLL.ViewModels.Home
{
    public record HomeIndexVM
    {
        public IEnumerable<EventTypeListVM> EventTypes { get; set; } = Enumerable.Empty<EventTypeListVM>();
        public IEnumerable<EventListVM> Events { get; set; } = Enumerable.Empty<EventListVM>();
        public string? SearchTerm { get; set; }
    }
}