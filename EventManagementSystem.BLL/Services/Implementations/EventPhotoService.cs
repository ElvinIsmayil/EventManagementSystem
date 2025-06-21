using AutoMapper;
using EventManagementSystem.BLL.Infrastructure.Interfaces;
using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.EventPhoto;
using EventManagementSystem.DAL.Entities;
using EventManagementSystem.DAL.Repositories.Interfaces;

namespace EventManagementSystem.BLL.Services.Implementations
{
    public class EventPhotoService : IEventPhotoService
    {
        private readonly IEventPhotoRepository _repository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        private const string FolderName = "event_photos";

        public EventPhotoService(IEventPhotoRepository repository, IImageService imageService, IMapper mapper)
        {
            _repository = repository;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<EventPhotoDetailsVM> AddAsync(int eventId, EventPhotoCreateVM model)
        {
            if (model.PhotoFile == null || model.PhotoFile.Length == 0)
            {
                throw new ArgumentException("Photo file is required.", nameof(model.PhotoFile));
            }

            (string photoUrl, List<string>? errors) = await _imageService.SaveImageAsync(model.PhotoFile, FolderName);

            if (errors != null && errors.Any())
            {
                throw new InvalidOperationException($"Failed to save image for event {eventId}: {string.Join(", ", errors)}");
            }

            var photo = _mapper.Map<EventPhoto>(model);
            photo.EventId = eventId;
            photo.Url = photoUrl;

            var addedPhoto = await _repository.AddAsync(photo);
            if (addedPhoto == null)
            {
                try
                {
                    _imageService.DeleteImage(photoUrl);
                }
                catch (Exception)
                {
                }
                throw new InvalidOperationException("Failed to save photo details to database.");
            }

            return _mapper.Map<EventPhotoDetailsVM>(addedPhoto);
        }

        public async Task<bool> DeleteAsync(int photoId)
        {
            var photo = await _repository.GetByIdAsync(photoId);
            if (photo == null)
            {
                return false;
            }

            var dbDeleteResult = await _repository.DeleteAsync(photoId);
            if (!dbDeleteResult)
            {
                return false;
            }

            try
            {
                _imageService.DeleteImage(photo.Url);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<EventPhotoDetailsVM?> GetByIdAsync(int photoId)
        {
            var photo = await _repository.GetByIdAsync(photoId);
            if (photo == null)
            {
                return null;
            }
            return _mapper.Map<EventPhotoDetailsVM>(photo);
        }

        public async Task<IEnumerable<EventPhotoDetailsVM>> GetPhotosByEventIdAsync(int eventId)
        {
            var photos = await _repository.GetPhotosByEventIdAsync(eventId);
            if (photos == null || !photos.Any())
            {
                return Enumerable.Empty<EventPhotoDetailsVM>();
            }
            var photoDetails = _mapper.Map<IEnumerable<EventPhotoDetailsVM>>(photos);
            return photoDetails;
        }

        public async Task<EventPhotoDetailsVM?> UpdateAsync(EventPhotoUpdateVM model)
        {
            var existingPhoto = await _repository.GetByIdAsync(model.Id);
            if (existingPhoto == null)
            {
                return null;
            }

            if (model.PhotoFile != null && model.PhotoFile.Length > 0)
            {
                (string newPhotoUrl, List<string>? errors) = await _imageService.SaveImageAsync(model.PhotoFile, FolderName);

                if (errors != null && errors.Any())
                {
                    throw new InvalidOperationException($"Failed to save new image for photo {model.Id}: {string.Join(", ", errors)}");
                }

                if (!string.IsNullOrEmpty(existingPhoto.Url))
                {
                    try
                    {
                        _imageService.DeleteImage(existingPhoto.Url);
                    }
                    catch (Exception)
                    {
                    }
                }

                existingPhoto.Url = newPhotoUrl;
            }

            _mapper.Map(model, existingPhoto);

            var updatedPhoto = await _repository.UpdateAsync(existingPhoto);
            if (updatedPhoto == null)
            {
                throw new InvalidOperationException("Failed to update photo details in database.");
            }

            return _mapper.Map<EventPhotoDetailsVM>(updatedPhoto);
        }
    }
}
