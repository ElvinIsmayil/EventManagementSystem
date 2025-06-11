using AutoMapper;
using EventManagementSystem.BLL.Infrastructure.Interfaces; 
using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.LocationPhoto;
using EventManagementSystem.DAL.Entities; 
using EventManagementSystem.DAL.Repositories.Interfaces; 

namespace EventManagementSystem.BLL.Services.Implementations
{
    public class LocationPhotoService : ILocationPhotoService
    {
        private readonly ILocationPhotoRepository _locationPhotoRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private const string ImageFolder = "locationPhotos"; 

        public LocationPhotoService(
            ILocationPhotoRepository locationPhotoRepository,
            IImageService imageService,
            IMapper mapper)
        {
            _locationPhotoRepository = locationPhotoRepository;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<LocationPhotoListVM?> AddPhotoToLocationAsync(int locationId, LocationPhotoCreateVM viewModel)
        {
            if (viewModel?.File == null)
            {
                return null; 
            }
            // 1. Save the physical file using IImageService
            // Correctly destructure the tuple and pass the folderName
            var (imageUrl, validationErrors) = await _imageService.SaveImageAsync(viewModel.File, ImageFolder);

            // Check for validation errors or if imageUrl is null/empty
            if (validationErrors != null && validationErrors.Any() || string.IsNullOrWhiteSpace(imageUrl))
            {
                return null; 
            }

            // 2. Create the LocationPhoto entity
            var locationPhoto = new LocationPhoto
            {
                LocationId = locationId,
                Url = imageUrl,
                Description = viewModel.Description,
                Order = viewModel.Order,
                CreatedAt = DateTime.UtcNow
            };

            // 3. Add the entity to the database
            var addedPhoto = await _locationPhotoRepository.AddAsync(locationPhoto);
            if (addedPhoto == null)
            {
                _imageService.DeleteImage(imageUrl); 
                return null;
            }

            // 4. Map and return the List VM
            return _mapper.Map<LocationPhotoListVM>(addedPhoto);
        }

        public async Task<bool> DeletePhotoFromLocationAsync(int photoId, int locationId)
        {
            var photoToDelete = await _locationPhotoRepository.GetByIdAsync(photoId);

            if (photoToDelete == null || photoToDelete.LocationId != locationId)
            {
                return false; 
            }

            // 1. Delete the physical file
            _imageService.DeleteImage(photoToDelete.Url);

            // 2. Delete the database record
            var result = await _locationPhotoRepository.DeleteAsync(photoToDelete.Id);
            return result;
        }

        public async Task<List<LocationPhotoListVM>> GetAllPhotosAsync()
        {
            var photos = await _locationPhotoRepository.GetAllAsync();
            return _mapper.Map<List<LocationPhotoListVM>>(photos);
        }

        public async Task<List<LocationPhotoListVM>> GetPhotosByLocationIdAsync(int locationId)
        {
            var photos = await _locationPhotoRepository.GetPhotosByLocationIdAsync(locationId);
            return _mapper.Map<List<LocationPhotoListVM>>(photos.OrderBy(p => p.Order).ToList());
        }

        public async Task<LocationPhotoListVM?> UpdatePhotoAsync(LocationPhotoUpdateVM viewModel)
        {
            if (viewModel == null) return null;

            var existingPhoto = await _locationPhotoRepository.GetByIdAsync(viewModel.Id);

            if (existingPhoto == null)
            {
                return null; 
            }

            string? newUrl = existingPhoto.Url; 

            if (viewModel.NewFile != null)
            {
                // Correctly destructure the tuple and pass folderName and oldImageUrl
                var (newImageUrlResult, validationErrors) = await _imageService.SaveImageAsync(viewModel.NewFile, ImageFolder, existingPhoto.Url);

                // Check for validation errors or if newImageUrlResult is null/empty
                if (validationErrors != null && validationErrors.Any() || string.IsNullOrWhiteSpace(newImageUrlResult))
                {
                    return null; // File save failed due to validation or server error in ImageService
                }
                newUrl = newImageUrlResult;
            }

            // Update entity properties
            existingPhoto.Url = newUrl;
            existingPhoto.Description = viewModel.Description;
            existingPhoto.Order = viewModel.Order;
            existingPhoto.UpdatedAt = DateTime.UtcNow; // Assuming UpdatedAt exists on BaseEntity or LocationPhoto

            var updatedPhoto = await _locationPhotoRepository.UpdateAsync(existingPhoto);

            if (updatedPhoto == null)
            {
                // Handle database update failure. If a new file was uploaded, consider deleting it here.
                // (This would require storing 'newUrl' temporarily outside the if block and deleting it if 'updatedPhoto' is null)
                return null;
            }

            return _mapper.Map<LocationPhotoListVM>(updatedPhoto);
        }
    }
}