namespace EventManagementSystem.BLL.ViewModels.LocationPhoto
{
    public record LocationPhotoDetailsVM
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Order { get; set; }
        public int LocationId { get; set; }
    }
}
