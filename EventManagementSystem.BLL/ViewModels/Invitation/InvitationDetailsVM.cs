namespace EventManagementSystem.BLL.ViewModels.Invitation
{
    public record InvitationDetailsVM
    {
        public int Id { get; init; }
        public string InvitationCode { get; init; } = string.Empty;
        public DateTime SentAt { get; init; }
        public string Status { get; init; } = string.Empty;
        public string EventName { get; init; } = string.Empty;
        public string EventDescription { get; init; } = string.Empty;
        public string PersonFullName { get; init; } = string.Empty;
        public string PersonEmail { get; init; } = string.Empty;

    }
}
