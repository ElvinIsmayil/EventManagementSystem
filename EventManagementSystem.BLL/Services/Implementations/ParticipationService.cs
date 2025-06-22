using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.BLL.Services.Implementations
{
    public class ParticipationService : IParticipationService
    {
        private readonly IParticipationRepository _participationRepository;

        public ParticipationService(IParticipationRepository participationRepository)
        {
            _participationRepository = participationRepository;
        }

        public Task<bool> AddParticipantAsync(int eventId, int personId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<int>> GetEventParticipantsAsync(int eventId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetParticipantCountAsync(int eventId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<int>> GetPersonEventsAsync(int personId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsParticipantAsync(int eventId, int personId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveParticipantAsync(int eventId, int personId)
        {
            throw new NotImplementedException();
        }

     

    }
}
