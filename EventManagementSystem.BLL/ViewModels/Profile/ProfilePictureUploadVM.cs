using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.BLL.ViewModels.Profile
{
    public record ProfilePictureUploadVM
    {
        public IFormFile? NewImageFile { get; set; }
        public string? CurrentImageUrl { get; set; }
    }
}
