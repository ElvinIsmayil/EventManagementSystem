using EventManagementSystem.BLL.ViewModels.EventPhoto;
using EventManagementSystem.BLL.ViewModels.EventType;
using EventManagementSystem.BLL.ViewModels.Location;
using EventManagementSystem.BLL.ViewModels.Organizer;

namespace EventManagementSystem.BLL.ViewModels.Event
{
    public record EventDetailsVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LocationDetailsVM Location { get; set; } = null!;
        public OrganizerDetailsVM Organizer { get; set; } = null!;
        public EventTypeDetailsVM EventType { get; set; } = null!;
        public List<EventPhotoDetailsVM> EventPhotos { get; set; } = new List<EventPhotoDetailsVM>();
        public int InvitationsCount { get; set; }
        public int ParticipationsCount { get; set; }
        public int FeedbacksCount { get; set; }
    }
}
