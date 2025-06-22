using AutoMapper;
using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.Event;
using EventManagementSystem.DAL.Entities;
using EventManagementSystem.DAL.Repositories.Interfaces;

namespace EventManagementSystem.BLL.Services.Implementations
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _repository;
        private readonly IEventPhotoService _eventPhotoService;
        private readonly IMapper _mapper;

        public EventService(IEventPhotoService eventPhotoService, IEventRepository repository, IMapper mapper)
        {
            _eventPhotoService = eventPhotoService;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EventDetailsVM> AddAsync(EventCreateVM model)
        {
            var eventEntity = _mapper.Map<Event>(model);
            if (model.EventPhotos != null)
            {
                foreach (var photo in model.EventPhotos)
                {
                    var eventPhoto = await _eventPhotoService.AddAsync(eventEntity.Id, photo);
                    if (eventPhoto == null)
                    {
                        Console.WriteLine($"Failed to add photo for location {eventEntity.Id}. Photo might be null or invalid.");
                        continue;
                    }
                    var eventPhotoEntity = _mapper.Map<EventPhoto>(eventPhoto);
                    eventEntity.EventPhotos.Add(eventPhotoEntity); // Assuming EventPhotos is a collection in Event entity
                }

            }
            var addedEvent = await _repository.AddAsync(eventEntity);
            if (addedEvent == null)
            {
                throw new InvalidOperationException("Failed to add event to the database.");
            }
            return _mapper.Map<EventDetailsVM>(addedEvent);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var eventToDelete = await _repository.GetByIdAsync(id);
            if (eventToDelete == null)
            {
                return false;
            }
            foreach (var photo in eventToDelete.EventPhotos)
            {
                try
                {
                    var photoDeletionResult = await _eventPhotoService.DeleteAsync(photo.Id);
                    if (!photoDeletionResult)
                    {
                        Console.WriteLine($"Failed to delete photo {photo.Id} for event {id}. Photo might not exist or an issue occurred at the data layer.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting photo {photo.Id} for event {id}: {ex.Message}");
                }
            }
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<EventListVM>> GetAllAsync()
        {
            var events = await _repository.GetAllAsync();
            if (events == null || !events.Any())
            {
                return Enumerable.Empty<EventListVM>();
            }
            var eventListVMs = _mapper.Map<IEnumerable<EventListVM>>(events);
            return eventListVMs;
        }

        public async Task<EventDetailsVM?> GetByIdAsync(int id)
        {
            var eventEntity = await _repository.GetByIdAsync(id);
            if (eventEntity == null)
            {
                return null;
            }
            var eventDetailsVM = _mapper.Map<EventDetailsVM>(eventEntity);
            return eventDetailsVM;
        }

        public async Task<IEnumerable<EventListVM>> GetPastEventsAsync(DateTime startDate, DateTime endDate)
        {
            var pastEvents = await _repository.GetPastEventsAsync(startDate, endDate);

            if (pastEvents == null || !pastEvents.Any())
            {
                return Enumerable.Empty<EventListVM>();
            }
            var eventListVMs = _mapper.Map<IEnumerable<EventListVM>>(pastEvents);
            return eventListVMs;
        }

        public async Task<IEnumerable<EventListVM>> GetUpcomingEventsAsync(DateTime startDate, DateTime endDate)
        {
            var upcomingEvents = await _repository.GetUpcomingEventsAsync(startDate, endDate);
            if (upcomingEvents == null || !upcomingEvents.Any())
            {
                return Enumerable.Empty<EventListVM>();
            }
            var eventListVMs = _mapper.Map<IEnumerable<EventListVM>>(upcomingEvents);
            return eventListVMs;

        }

        public async Task<EventUpdateVM> GetUpdateByIdAsync(int id)
        {
            var eventEntity = await _repository.GetByIdAsync(id);
            if (eventEntity == null)
            {
                throw new KeyNotFoundException($"Event with ID {id} not found.");
            }
            var eventUpdateVM = _mapper.Map<EventUpdateVM>(eventEntity);
            return eventUpdateVM;
        }

        public async Task<IEnumerable<EventListVM>> SearchEventsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                throw new ArgumentException("Search term cannot be null or empty.", nameof(searchTerm));
            }
            var events = await _repository.SearchEventsAsync(searchTerm);
            if (events == null || !events.Any())
            {
                return Enumerable.Empty<EventListVM>();
            }
            var eventListVMs = _mapper.Map<IEnumerable<EventListVM>>(events);
            return eventListVMs;
        }

        public async Task<EventDetailsVM> UpdateAsync(EventUpdateVM model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Event update model cannot be null.");
            }
            var eventEntity = _mapper.Map<Event>(model);
            if (model.EventPhotos != null)
            {
                foreach (var photo in model.EventPhotos)
                {
                    var eventPhoto = _eventPhotoService.UpdateAsync(photo);
                    if (eventPhoto == null)
                    {
                        Console.WriteLine($"Failed to update photo for event {eventEntity.Id}. Photo might be null or invalid.");
                        continue;
                    }
                    var eventPhotoEntity = _mapper.Map<EventPhoto>(eventPhoto);
                    eventEntity.EventPhotos.Add(eventPhotoEntity); // Assuming EventPhotos is a collection in Event entity
                }
            }
            var updatedEvent = _repository.UpdateAsync(eventEntity);
            if (updatedEvent == null)
            {
                throw new InvalidOperationException("Failed to update event in the database.");
            }
            return _mapper.Map<EventDetailsVM>(updatedEvent);
        }

        public async Task<IEnumerable<EventListVM>> GetEventsByOrganizerIdAsync(int organizerId)
        {
            if (organizerId <= 0)
            {
                throw new ArgumentException("Organizer ID cannot be null or empty.", nameof(organizerId));
            }
            var events = await _repository.GetEventsByOrganizerIdAsync(organizerId);
            if (events == null || !events.Any())
            {
                return Enumerable.Empty<EventListVM>();
            }
            var eventListVMs = _mapper.Map<IEnumerable<EventListVM>>(events);
            return eventListVMs;
        }
    }
}



