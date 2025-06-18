namespace EventManagementSystem.BLL.ViewModels.LocationPhoto
{
    public record LocationPhotoListVM
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public int Order { get; set; }
    }
}
