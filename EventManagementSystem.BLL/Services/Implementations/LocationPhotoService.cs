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

        public async Task<LocationPhotoDetailsVM?> AddPhotoToLocationAsync(int locationId, LocationPhotoCreateVM viewModel)
        {
            if (viewModel?.File == null)
            {
                return null;
            }
            var (imageUrlResult, validationErrors) = await _imageService.SaveImageAsync(viewModel.File, ImageFolder);

            if (validationErrors != null && validationErrors.Any() || string.IsNullOrWhiteSpace(imageUrlResult))
            {
                return null;
            }
            var locationPhoto = _mapper.Map<LocationPhoto>(viewModel);
            locationPhoto.LocationId = locationId;
            locationPhoto.Url = imageUrlResult;

            var addedPhoto = await _locationPhotoRepository.AddAsync(locationPhoto);
            if (addedPhoto == null)
            {
                _imageService.DeleteImage(imageUrlResult);
                return null;
            }

            return _mapper.Map<LocationPhotoDetailsVM>(addedPhoto);
        }

        public async Task<bool> DeletePhotoFromLocationAsync(int photoId, int locationId)
        {
            var photoToDelete = await _locationPhotoRepository.GetByIdAsync(photoId);

            if (photoToDelete == null || photoToDelete.LocationId != locationId)
            {
                return false;
            }

            _imageService.DeleteImage(photoToDelete.Url);

            var result = await _locationPhotoRepository.DeleteAsync(photoToDelete.Id);
            return result;
        }

        public async Task<List<LocationPhotoDetailsVM>> GetAllPhotosAsync()
        {
            var photos = await _locationPhotoRepository.GetAllAsync();
            return _mapper.Map<List<LocationPhotoDetailsVM>>(photos);
        }

        public async Task<List<LocationPhotoDetailsVM>> GetPhotosByLocationIdAsync(int locationId)
        {
            var photos = await _locationPhotoRepository.GetPhotosByLocationIdAsync(locationId);
            return _mapper.Map<List<LocationPhotoDetailsVM>>(photos.OrderBy(p => p.Order).ToList());
        }

        public async Task<LocationPhotoDetailsVM?> UpdatePhotoAsync(LocationPhotoUpdateVM viewModel)
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
                var (newImageUrlResult, validationErrors) = await _imageService.SaveImageAsync(viewModel.NewFile, ImageFolder, existingPhoto.Url);

                if (validationErrors != null && validationErrors.Any() || string.IsNullOrWhiteSpace(newImageUrlResult))
                {
                    return null;
                }
                newUrl = newImageUrlResult;
            }

            existingPhoto.Url = newUrl;
            existingPhoto.Description = viewModel.Description;
            existingPhoto.Order = viewModel.Order;
            existingPhoto.UpdatedAt = DateTime.UtcNow;

            return _mapper.Map<LocationPhotoDetailsVM>(existingPhoto);
        }
    }
}