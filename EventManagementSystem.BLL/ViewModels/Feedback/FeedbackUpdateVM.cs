using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.BLL.ViewModels.Feedback
{
    public record FeedbackUpdateVM
    {
        [Required]
        public int Id { get; set; } // The ID of the Feedback entity to update

        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
        [DataType(DataType.MultilineText)]
        public string? Comment { get; set; }

    }
}
