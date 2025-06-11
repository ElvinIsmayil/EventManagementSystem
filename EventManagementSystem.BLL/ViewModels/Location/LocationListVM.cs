﻿namespace EventManagementSystem.BLL.ViewModels.Location
{
    public record LocationListVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string MainPhotoUrl { get; set; } = string.Empty;
    }
}
