using AutoMapper;
using EventManagementSystem.BLL.Infrastructure.Interfaces;
using EventManagementSystem.BLL.Services.Interfaces;
using EventManagementSystem.BLL.ViewModels.Location;
using EventManagementSystem.DAL.Repositories.Interfaces;

namespace EventManagementSystem.BLL.Services.Implementations
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _repository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;




        public LocationService(ILocationRepository repository, IImageService imageService, IMapper mapper)
        {
            _repository = repository;
            _imageService = imageService;
            _mapper = mapper;
        }

        public Task<LocationDetailsVM> AddAsync(LocationCreateVM viewModel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LocationListVM>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LocationListVM>> GetAllLocationsWithPhotosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<LocationDetailsVM?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<LocationUpdateVM?> GetUpdateByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LocationListVM>> SearchLocationsAsync(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public Task<LocationDetailsVM> UpdateAsync(LocationUpdateVM viewModel)
        {
            throw new NotImplementedException();
        }
    }
}
