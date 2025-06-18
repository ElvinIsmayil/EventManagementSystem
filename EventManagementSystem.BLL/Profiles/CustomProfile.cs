using AutoMapper;
using EventManagementSystem.BLL.ViewModels.EventType;
using EventManagementSystem.BLL.ViewModels.Location;
using EventManagementSystem.BLL.ViewModels.LocationPhoto;
using EventManagementSystem.BLL.ViewModels.User;
using EventManagementSystem.DAL.Entities;

namespace EventManagementSystem.BLL.Profiles
{
    public class CustomProfile : Profile
    {
        public CustomProfile()
        {
            CreateMap<EventType, EventTypeListVM>()
                 .ForMember(dest => dest.EventCount, opt => opt.MapFrom(src => src.Events.Count()))
                 .ReverseMap();
            CreateMap<EventType, EventTypeCreateVM>().ReverseMap();
            CreateMap<EventType, EventTypeUpdateVM>().ReverseMap();
            CreateMap<EventType, EventTypeDetailsVM>().ReverseMap();

            CreateMap<Location, LocationListVM>()
                .ForMember(dest => dest.MainPhotoUrl, opt => opt.MapFrom(src =>
                    src.locationPhotos != null && src.locationPhotos.Any()
                        ? src.locationPhotos.OrderBy(p => p.Order).FirstOrDefault()!.Url 
                        : string.Empty 
                ))
                .ReverseMap(); 

            CreateMap<Location, LocationCreateVM>().ReverseMap();
            CreateMap<Location, LocationUpdateVM>().ReverseMap();
            CreateMap<Location, LocationDetailsVM>().ReverseMap();

            CreateMap<LocationPhoto, LocationPhotoListVM>().ReverseMap();
            CreateMap<LocationPhoto, LocationPhotoCreateVM>().ReverseMap();
            CreateMap<LocationPhoto, LocationPhotoUpdateVM>().ReverseMap();
            CreateMap<LocationPhoto, LocationPhotoDetailsVM>().ReverseMap();

            CreateMap<AppUser, UserListVM>()
              .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src =>
                  string.IsNullOrEmpty(src.ImageUrl)
                  ? "/media/avatars/blank.png"
                  : src.ImageUrl
              ))
              .ReverseMap()
              .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            CreateMap<AppUser, UserCreateVM>().ReverseMap();
            CreateMap<AppUser, UserUpdateVM>().ReverseMap();

            CreateMap<AppUser, UserDetailsVM>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.ImageUrl)
                    ? "/media/avatars/blank.png"
                    : src.ImageUrl
                ))
                .ReverseMap()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
        }
    } }
