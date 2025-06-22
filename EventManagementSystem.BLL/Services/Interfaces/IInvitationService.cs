using EventManagementSystem.BLL.ViewModels.Invitation;
using EventManagementSystem.DAL.Enums;

public interface IInvitationService
{
    Task<InvitationDetailsVM> CreateInvitationAsync(InvitationCreateVM model);
    Task<IEnumerable<InvitationListVM>> GetAllAsync();
    Task<InvitationDetailsVM?> GetByIdAsync(int id);
    Task<InvitationDetailsVM?> UpdateByAsync(InvitationUpdateVM model);
    Task<bool> DeleteAsync(int id);

    Task RespondToInvitationAsync(string invitationCode, string appUserId);

    Task SendInvitationAsync(int eventId); // sends invitation to all relevant users (e.g., all students)

    Task<IEnumerable<InvitationListVM>> GetEventInvitationsAsync(int eventId);
    Task<IEnumerable<InvitationListVM>> GetPersonInvitationsAsync(int personId);

    Task<InvitationDetailsVM> GetInvitationByCodeAsync(string invitationCode);

    Task UpdateInvitationStatusAsync(string invitationCode, InvitationStatus status);
}
