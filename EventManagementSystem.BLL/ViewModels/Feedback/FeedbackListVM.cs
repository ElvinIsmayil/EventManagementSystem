using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.BLL.ViewModels.Feedback
{
    public record FeedbackListVM
    {
        public int Id { get; set; }

        public int Rating { get; set; }

        [Display(Name = "Comment Preview")]
        public string? CommentPreview { get; set; } // A shortened version of the comment for list view

        [Display(Name = "Submitted At")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime SubmittedAt { get; set; }

        // Display properties for related entities (Person and Event)
        [Display(Name = "Submitted By")]
        public string PersonFullName { get; set; } = string.Empty; // From Person.AppUser.FullName

        [Display(Name = "For Event")]
        public string EventName { get; set; } = string.Empty;
    }
}
