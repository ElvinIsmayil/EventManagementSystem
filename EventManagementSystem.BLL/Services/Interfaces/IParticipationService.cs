using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.BLL.Services.Interfaces
{
    public interface IParticipationService
    {
        Task<bool> AddParticipantAsync(int eventId, int personId);
        Task<bool> RemoveParticipantAsync(int eventId, int personId);
        Task<IEnumerable<int>> GetEventParticipantsAsync(int eventId);
        Task<IEnumerable<int>> GetPersonEventsAsync(int personId);
        Task<bool> IsParticipantAsync(int eventId, int personId);
        Task<int> GetParticipantCountAsync(int eventId);
       // Task<bool> CheckInAsync(int eventId, int personId);


    }
}
