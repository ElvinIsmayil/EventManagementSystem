using AutoMapper;
using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.Location;
using EventManagementSystem.DAL.Entities;
using EventManagementSystem.DAL.Repositories.Interfaces;

namespace EventManagementSystem.BLL.Services.Implementations
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _repository;
        private readonly ILocationPhotoService _locationPhotoService;
        private readonly IMapper _mapper;

        public LocationService(ILocationRepository repository, ILocationPhotoService locationPhotoService, IMapper mapper)
        {
            _repository = repository;
            _locationPhotoService = locationPhotoService;
            _mapper = mapper;
        }

        public async Task<LocationDetailsVM> AddAsync(LocationCreateVM viewModel)
        {
            var location = _mapper.Map<Location>(viewModel);
            var addedLocation = await _repository.AddAsync(location);
            if (addedLocation == null)
            {
                return null;
            }

            if (viewModel.Photos != null && viewModel.Photos.Any())
            {
                foreach (var photoVM in viewModel.Photos)
                {
                    var addedPhoto = await _locationPhotoService.AddPhotoToLocationAsync(addedLocation.Id, photoVM);
                    if (addedPhoto == null)
                    {
                        Console.WriteLine($"Failed to add photo for location {addedLocation.Id}. Photo might be null or invalid.");
                        continue;
                    }
                }
            }
            return _mapper.Map<LocationDetailsVM>(addedLocation);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var locationToDelete = await _repository.GetByIdWithPhotosAsync(id);
            if (locationToDelete == null)
            {
                return false;
            }
            foreach (var photo in locationToDelete.locationPhotos)
            {
                try
                {
                    var photoDeletionResult = await _locationPhotoService.DeletePhotoFromLocationAsync(photo.Id, id);
                    if (!photoDeletionResult)
                    {
                        Console.WriteLine($"Failed to delete photo {photo.Id} for location {id}. Photo might not exist or an issue occurred at the data layer.");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting photo {photo.Id} for location {id}: {ex.Message}");
                }
            }
            var result = await _repository.DeleteAsync(id);
            if (!result)
            {
                Console.WriteLine($"Failed to delete location with ID {id}. It might be in use or protected.");
            }
            return result;
        }
        public async Task<IEnumerable<LocationListVM>> GetAllAsync()
        {
            var locations = await _repository.GetAllAsync();
            if (locations == null || !locations.Any())
            {
                return Enumerable.Empty<LocationListVM>();
            }
            var locationListVMs = _mapper.Map<IEnumerable<LocationListVM>>(locations);
            return locationListVMs;
        }

        public async Task<IEnumerable<LocationListVM>> GetAllLocationsWithPhotosAsync()
        {
            var locations = await _repository.GetAllLocationsWithPhotosAsync();
            if (locations == null || !locations.Any())
            {
                return Enumerable.Empty<LocationListVM>();
            }
            return _mapper.Map<IEnumerable<LocationListVM>>(locations);
        }

        public async Task<LocationDetailsVM?> GetByIdAsync(int id)
        {
            var location = await _repository.GetByIdWithPhotosAndEventsAsync(id);
            if (location == null)
            {
                return null;
            }
            var locationVM = _mapper.Map<LocationDetailsVM>(location);
            return locationVM;
        }

        public async Task<LocationUpdateVM?> GetUpdateByIdAsync(int id)
        {
            var location = await _repository.GetByIdAsync(id);
            if (location == null)
            {
                return null;
            }
            var locationUpdateVM = _mapper.Map<LocationUpdateVM>(location);
            return locationUpdateVM;

        }

        public Task<IEnumerable<LocationListVM>> SearchLocationsAsync(string searchTerm)
        {
            throw new NotImplementedException("SearchLocationsAsync method is not implemented yet.");
        }

        public async Task<LocationDetailsVM> UpdateAsync(LocationUpdateVM viewModel)
        {
            var locationToUpdate = _mapper.Map<Location>(viewModel);
            var updatedLocation = await _repository.UpdateAsync(locationToUpdate);
            if (updatedLocation == null)
            {
                return null;
            }
            if (viewModel.LocationPhotos != null && viewModel.LocationPhotos.Any())
            {
                foreach (var photoVM in viewModel.LocationPhotos)
                {
                    var updatedPhoto = await _locationPhotoService.UpdatePhotoAsync(photoVM);
                    if (updatedPhoto == null)
                    {
                        Console.WriteLine($"Failed to update photo for location {updatedLocation.Id}. Photo might be null or invalid.");
                        continue;
                    }
                }
            }
            return _mapper.Map<LocationDetailsVM>(updatedLocation);
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
