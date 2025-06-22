namespace EventManagementSystem.BLL.ViewModels.Invitation
{
    public record InvitationListVM
    {
        public int Id { get; init; }
        public string InvitationCode { get; init; } = string.Empty;
        public DateTime SentAt { get; init; } = DateTime.UtcNow;
        public string PersonFullName { get; init; } = string.Empty;
        public string EventName { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;

    }
}
