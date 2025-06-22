using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.BLL.ViewModels.Feedback
{
    public record FeedbackDetailsVM
    {
        public int Id { get; set; }

        [Display(Name = "Rating (1-5)")]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        [Display(Name = "Submitted At")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime SubmittedAt { get; set; }

        // Detailed properties for related entities
        [Display(Name = "Submitted By")]
        public int PersonId { get; set; }
        public string PersonFullName { get; set; } = string.Empty; // From Person.AppUser.FullName
        public string PersonEmail { get; set; } = string.Empty;     // From Person.AppUser.Email

        [Display(Name = "For Event")]
        public int EventId { get; set; }
        public string EventName { get; set; } = string.Empty;     // From Event.Name
        public DateTime EventStartDate { get; set; }
    }
}
