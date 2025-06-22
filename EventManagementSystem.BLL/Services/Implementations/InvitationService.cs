using AutoMapper;
using EventManagementSystem.BLL.Infrastructure;
using EventManagementSystem.BLL.Infrastructure.Interfaces;
using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.Invitation;
using EventManagementSystem.DAL.Entities;
using EventManagementSystem.DAL.Enums;
using EventManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.BLL.Services.Implementations
{
    public class InvitationService : IInvitationService
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IPersonRepository _personRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEventRepository _eventRepository;
        private readonly IParticipationRepository _participationRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public InvitationService(
            IInvitationRepository invitationRepository,
            IMapper mapper,
            IEventRepository eventRepository,
            IPersonRepository personRepository,
            IEmailService emailService,
            IParticipationRepository participationRepository,
            UserManager<AppUser> userManager)
        {
            _invitationRepository = invitationRepository;
            _mapper = mapper;
            _eventRepository = eventRepository;
            _personRepository = personRepository;
            _emailService = emailService;
            _participationRepository = participationRepository;
            _userManager = userManager;
        }

        // Create new invitation
        public async Task<InvitationDetailsVM> CreateInvitationAsync(InvitationCreateVM model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var invitation = _mapper.Map<Invitation>(model);
            var addedInvitation = await _invitationRepository.AddAsync(invitation);
            if (addedInvitation == null)
                throw new InvalidOperationException("Failed to create invitation.");

            return _mapper.Map<InvitationDetailsVM>(addedInvitation);
        }

        // Delete invitation by Id
        public async Task<bool> DeleteAsync(int id)
        {
            var invitation = await _invitationRepository.GetByIdAsync(id);
            if (invitation == null)
                throw new KeyNotFoundException($"Invitation with ID {id} not found.");

            var result = await _invitationRepository.DeleteAsync(id);
            if (!result)
                throw new InvalidOperationException($"Failed to delete invitation with ID {id}.");

            return true;
        }

        // Get all invitations
        public async Task<IEnumerable<InvitationListVM>> GetAllAsync()
        {
            var invitations = await _invitationRepository.GetAllAsync();
            return invitations == null || !invitations.Any()
                ? new List<InvitationListVM>()
                : _mapper.Map<IEnumerable<InvitationListVM>>(invitations);
        }

        // Get invitation by Id
        public async Task<InvitationDetailsVM?> GetByIdAsync(int id)
        {
            var invitation = await _invitationRepository.GetByIdAsync(id);
            if (invitation == null)
                throw new KeyNotFoundException($"Invitation with ID {id} not found.");

            return _mapper.Map<InvitationDetailsVM>(invitation);
        }

        // Get invitations for an event
        public async Task<IEnumerable<InvitationListVM>> GetEventInvitationsAsync(int eventId)
        {
            var invitations = await _invitationRepository.GetByEventIdAsync(eventId);
            if (invitations == null || !invitations.Any())
                throw new InvalidOperationException($"No invitations found for event ID {eventId}.");

            return _mapper.Map<IEnumerable<InvitationListVM>>(invitations);
        }

        // Get invitation by invitation code (string)
        public async Task<InvitationDetailsVM> GetInvitationByCodeAsync(string invitationCode)
        {
            var invitation = await _invitationRepository.GetByCodeAsync(invitationCode);
            if (invitation == null)
                throw new KeyNotFoundException($"Invitation with code {invitationCode} not found.");

            return _mapper.Map<InvitationDetailsVM>(invitation);
        }

        // Get invitations for a person
        public async Task<IEnumerable<InvitationListVM>> GetPersonInvitationsAsync(int personId)
        {
            var invitations = await _invitationRepository.GetByPersonIdAsync(personId);
            return invitations == null || !invitations.Any()
                ? new List<InvitationListVM>()
                : _mapper.Map<IEnumerable<InvitationListVM>>(invitations);
        }

        public async Task SendInvitationAsync(int eventId)
        {
            if (eventId <= 0)
                throw new ArgumentException("Invalid event id");

            var eventEntity = await _eventRepository.GetWithLocationAsync(eventId);
            if (eventEntity == null)
                throw new KeyNotFoundException("Event not found");

            var studentUsers = await _userManager.GetUsersInRoleAsync("Student");
            var studentUserIds = studentUsers.Select(u => u.Id).ToList();

            var persons = await _personRepository.GetWithInvitationsAndNotificationsAsync(studentUserIds);

            foreach (var user in persons)
            {
                var newInvitation = new Invitation
                {
                    EventId = eventId,
                    PersonId = user.Id,
                    InvitationCode = Guid.NewGuid().ToString("N"),
                    Status = InvitationStatus.Pending,
                    SentAt = DateTime.UtcNow
                };

                user.Invitations.Add(newInvitation);

                var newNotification = new Notification
                {
                    PersonId = user.Id,
                    Message = $"You have been invited to {eventEntity.Title}.",
                    CreatedAt = DateTime.UtcNow,
                    EventId = eventId,
                    RecipientEmail = user.AppUser.Email,
                    Status = NotificationStatus.Pending,
                    ScheduledAt = DateTime.UtcNow.AddMinutes(5)
                };

                user.Notifications.Add(newNotification);

                string invitationLink = $"https://localhost:7170/Invitations/Respond?code={newInvitation.InvitationCode}";
                await _userManager.UpdateAsync(user.AppUser); // Consider if this `UpdateAsync` is needed here. It primarily updates the AppUser entity, not related navigation properties which would be saved with `_personRepository.UpdateAsync` later.

                await _emailService.SendEmailAsync(user.AppUser.Email,
                    $"Invitation to {eventEntity.Title}",
                    $"You have been invited to {eventEntity.Title}. Click here to respond: {invitationLink}");
            }
        }

        // Respond to invitation by invitation code (accept, create participation, notify, send ticket)
        public async Task RespondToInvitationAsync(string invitationCode, string appUserId)
        {
            if (string.IsNullOrEmpty(invitationCode))
                throw new ArgumentException("Invitation code is required.");
            if (string.IsNullOrEmpty(appUserId))
                throw new ArgumentException("AppUserId is required.");

            var user = await _personRepository.GetByAppUserIdWithInvitationsAndNotificationsAsync(appUserId)
                ?? throw new KeyNotFoundException("Person not found.");

            var invitation = user.Invitations.FirstOrDefault(i => i.InvitationCode == invitationCode)
                ?? throw new KeyNotFoundException("Invitation not found for this user.");

            var eventEntity = await _eventRepository.GetWithLocationAsync(invitation.EventId)
                ?? throw new KeyNotFoundException("Event not found.");

            // Accept invitation
            invitation.Status = InvitationStatus.Accepted;
            await _invitationRepository.UpdateAsync(invitation);

            // Create participation
            var participation = new Participation
            {
                InvitationId = invitation.Id,
                ParticipationDate = DateTime.UtcNow,
                ConfirmationCode = GenerateConfirmationCode(),
                SeatNumber = new Random().Next(1, 200), // Consider a more robust seat assignment
                IsConfirmed = true
            };
            await _participationRepository.AddAsync(participation);

            // Create notification
            var newNotification = new Notification
            {
                PersonId = user.Id,
                EventId = eventEntity.Id,
                Message = $"You have successfully accepted the invitation to {eventEntity.Title}.",
                CreatedAt = DateTime.UtcNow,
                ScheduledAt = DateTime.UtcNow
            };
            user.Notifications.Add(newNotification);

            await _personRepository.UpdateAsync(user);

            // Generate and send ticket email using the helper
            var ticketHtml = EmailTemplateHelper.GenerateTicketHtml( // Using the helper
                eventEntity.Title,
                eventEntity.StartDate, // Pass the DateTime object
                eventEntity.Location.Name,
                invitation.InvitationCode,
                user.AppUser.Fullname,
                participation.ConfirmationCode,
                participation.SeatNumber.ToString()
            );

            await _emailService.SendEmailAsync(
                user.AppUser.Email,
                subject: $"Your Ticket for {eventEntity.Title}",
                message: ticketHtml
            );
        }

        // Update invitation by model
        public async Task<InvitationDetailsVM?> UpdateByAsync(InvitationUpdateVM model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var invitation = await _invitationRepository.GetByIdAsync(model.Id)
                ?? throw new KeyNotFoundException($"Invitation with ID {model.Id} not found.");

            invitation.Status = model.Status;

            var updatedInvitation = await _invitationRepository.UpdateAsync(invitation);
            if (updatedInvitation == null)
                throw new InvalidOperationException($"Failed to update invitation with ID {model.Id}.");

            return _mapper.Map<InvitationDetailsVM>(updatedInvitation);
        }

        public Task UpdateInvitationStatusAsync(string invitationCode, InvitationStatus status)
        {
            throw new NotImplementedException();
        }

        // Generate 6-digit numeric confirmation code string (remains in service as it's business logic)
        private string GenerateConfirmationCode()
        {
            var random = new Random();
            int code = random.Next(100000, 1000000); // 6-digit code 100000 - 999999
            return code.ToString();
        }
    }
}