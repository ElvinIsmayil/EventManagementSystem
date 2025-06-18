namespace EventManagementSystem.BLL.ViewModels.EventType
{
    public record EventTypeListVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EventCount { get; set; }
    }
}
