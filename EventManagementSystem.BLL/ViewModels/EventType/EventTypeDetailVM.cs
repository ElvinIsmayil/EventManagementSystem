namespace EventManagementSystem.BLL.ViewModels.EventType
{
    public record EventTypeDetailVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
