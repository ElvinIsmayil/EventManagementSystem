using AutoMapper;
using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.Organizer;
using EventManagementSystem.DAL.Entities;
using EventManagementSystem.DAL.Repositories.Interfaces;

namespace EventManagementSystem.BLL.Services.Implementations
{
    public class OrganizerService : IOrganizerService
    {
        private readonly IOrganizerRepository _repository;
        private readonly IMapper _mapper;

        public OrganizerService(IOrganizerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(OrganizerCreateVM model)
        {
            var organizer = _mapper.Map<Organizer>(model);
            if (organizer == null)
            {
                throw new ArgumentNullException(nameof(model), "Model cannot be null");
            }
            var addedOrganizer = await _repository.AddAsync(organizer);
            if (addedOrganizer == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> ApproveOrganizerAsync(int id)
        {
            var organizer = await _repository.GetByIdAsync(id);
            if (organizer == null)
            {
                return false;
            }
            organizer.IsApproved = true;
            var updatedOrganizer = await _repository.UpdateAsync(organizer);
            if (updatedOrganizer == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var organizer = await _repository.GetByIdAsync(id);
            if (organizer == null)
            {
                return false;
            }
            organizer.IsDeleted = true;
            var updatedOrganizer = await _repository.UpdateAsync(organizer);
            if (updatedOrganizer == null)
            {
                return false;
            }
            return true;
        }

        public async Task<IEnumerable<OrganizerListVM>> GetAllAsync()
        {
            var organizers = await _repository.GetAllAsync();
            if (organizers == null)
            {
                throw new InvalidOperationException("No organizers found.");
            }
            var organizerListVMs = _mapper.Map<IEnumerable<OrganizerListVM>>(organizers);
            return organizerListVMs;

        }

        public async Task<OrganizerDetailsVM?> GetByIdAsync(int id)
        {
            var organizer = await _repository.GetByIdAsync(id);
            if (organizer == null)
            {
                return null;
            }
            var organizerDetailsVM = _mapper.Map<OrganizerDetailsVM>(organizer);
            return organizerDetailsVM;
        }

        public async Task<OrganizerDetailsVM?> GetOrganizerByAppUserIdAsync(string appUserId)
        {
            var organizer = await _repository.GetOrganizerByAppUserIdAsync(appUserId);
            if (organizer == null)
            {
                return null;
            }
            var organizerDetailsVM = _mapper.Map<OrganizerDetailsVM>(organizer);
            return organizerDetailsVM;

        }

        public async Task<OrganizerUpdateVM?> GetUpdateByIdAsync(int id)
        {
            var organizer = await _repository.GetByIdAsync(id);
            if (organizer == null)
            {
                return null;
            }
            var organizerUpdateVM = _mapper.Map<OrganizerUpdateVM>(organizer);
            return organizerUpdateVM;
        }

        public Task<IEnumerable<OrganizerListVM>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                throw new ArgumentException("Search term cannot be null or empty.", nameof(searchTerm));
            }
            var organizers = _repository.SearchAsync(searchTerm);
            if (organizers == null)
            {
                throw new InvalidOperationException("No organizers found matching the search term.");
            }
            var organizerListVMs = _mapper.Map<IEnumerable<OrganizerListVM>>(organizers);
            return Task.FromResult(organizerListVMs);
        }

        public async Task<bool> UpdateAsync(OrganizerUpdateVM model)
        {
            var organizer = _mapper.Map<Organizer>(model);
            if (organizer == null)
            {
                throw new ArgumentNullException(nameof(model), "Model cannot be null");
            }
            var updatedOrganizer = await _repository.UpdateAsync(organizer);
            if (updatedOrganizer == null)
            {
                return false;
            }
            return true;


        }
    }
}
