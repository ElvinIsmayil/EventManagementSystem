using AutoMapper;
using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.EventType;
using EventManagementSystem.DAL.Entities;
using EventManagementSystem.DAL.Repositories.Interfaces;

namespace EventManagementSystem.BLL.Services.Implementations
{
    public class EventTypeService : IEventTypeService
    {
        private readonly IEventTypeRepository _repository;
        private readonly IMapper _mapper;

        public EventTypeService(IEventTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EventTypeDetailsVM> AddAsync(EventTypeCreateVM viewModel)
        {
            var entity = _mapper.Map<EventType>(viewModel);
            var addedEntity = await _repository.AddAsync(entity);

            var resultViewModel = _mapper.Map<EventTypeDetailsVM>(addedEntity);
            return resultViewModel;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
        public async Task<IEnumerable<EventTypeListVM>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            var viewModels = _mapper.Map<IEnumerable<EventTypeListVM>>(entities);
            return viewModels;
        }

        public async Task<EventTypeDetailsVM?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return null;
            }

            var viewModel = _mapper.Map<EventTypeDetailsVM>(entity);
            return viewModel;
        }

        public async Task<EventTypeDetailsVM> UpdateAsync(EventTypeUpdateVM viewModel)
        {
            var entityToUpdate = _mapper.Map<EventType>(viewModel);
            var updatedEntity = await _repository.UpdateAsync(entityToUpdate);
            var resultViewModel = _mapper.Map<EventTypeDetailsVM>(updatedEntity);
            return resultViewModel;
        }

        public async Task<EventTypeUpdateVM?> GetUpdateByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return null;
            }

            var updateViewModel = _mapper.Map<EventTypeUpdateVM>(entity);
            return updateViewModel;
        }

        public async Task<IEnumerable<EventTypeListVM>> SearchEventTypeAsync(string searchTerm)
        {
            var entities = await _repository.SearchEventTypeAsync(searchTerm);
            var viewModels = _mapper.Map<IEnumerable<EventTypeListVM>>(entities);
            return viewModels;
        }

        public async Task<(int deletedCount, List<string> failedDeletions)> DeleteMultipleAsync(List<int> ids)
        {
            int deletedCount = 0;
            List<string> failedDeletions = new List<string>();

            if (ids == null || !ids.Any())
            {
                return (0, failedDeletions);
            }

            foreach (var id in ids)
            {
                try
                {
                    var result = await DeleteAsync(id);
                    if (result)
                    {
                        deletedCount++;
                    }
                    else
                    {
                        failedDeletions.Add($"Failed to delete Event Type with ID {id}. Item might not exist or an issue occurred at the data layer.");
                    }
                }
                catch (Exception ex)
                {
                    failedDeletions.Add($"Error deleting Event Type with ID {id}: {ex.Message}");
                }
            }

            return (deletedCount, failedDeletions);
        }
    }
}
