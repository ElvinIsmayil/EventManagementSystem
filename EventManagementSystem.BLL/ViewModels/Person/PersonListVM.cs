using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.BLL.ViewModels.Person
{
    public record PersonListVM
    {
        public string AppUserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty; // From AppUser.Fullname
        public string Email { get; set; } = string.Empty;
    }
}
