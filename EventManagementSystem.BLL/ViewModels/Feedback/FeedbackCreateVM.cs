using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.BLL.ViewModels.Feedback
{
    public record FeedbackCreateVM
    {
        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
        [DataType(DataType.MultilineText)]
        public string? Comment { get; set; }

        // These IDs are crucial for linking the feedback
        [Required(ErrorMessage = "Person ID is required.")]
        public int PersonId { get; set; }

        [Required(ErrorMessage = "Event ID is required.")]
        public int EventId { get; set; }
    }
}
