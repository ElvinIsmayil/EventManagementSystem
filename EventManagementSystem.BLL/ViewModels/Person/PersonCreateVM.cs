using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.BLL.ViewModels.Person
{
    public record PersonCreateVM
    {
        [Required(ErrorMessage = "App User ID is required to link this person to a user account.")]
        public string AppUserId { get; set; } = string.Empty;
    }
}
