using AutoMapper;
using EventManagementSystem.BLL.ViewModels.Auth;
using EventManagementSystem.BLL.ViewModels.Event;
using EventManagementSystem.BLL.ViewModels.EventPhoto;
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
            //EventType mappings
            CreateMap<EventType, EventTypeListVM>()
                 .ForMember(dest => dest.EventCount, opt => opt.MapFrom(src => src.Events.Count()))
                 .ReverseMap();
            CreateMap<EventType, EventTypeCreateVM>().ReverseMap();
            CreateMap<EventType, EventTypeUpdateVM>().ReverseMap();
            CreateMap<EventType, EventTypeDetailsVM>().ReverseMap();


            //Location mappings  
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

            CreateMap<LocationPhoto, LocationPhotoCreateVM>().ReverseMap();
            CreateMap<LocationPhoto, LocationPhotoUpdateVM>().ReverseMap();
            CreateMap<LocationPhoto, LocationPhotoDetailsVM>().ReverseMap();

            //User mappings
            CreateMap<AppUser, SignUpVM>().ReverseMap();
            CreateMap<AppUser, SignInVM>().ReverseMap();

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


            //Event mappings
            CreateMap<Event, EventListVM>()
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location.Name))
                .ForMember(dest => dest.OrganizerName, opt => opt.MapFrom(src => src.Organizer.AppUser.Fullname))
                .ForMember(dest => dest.EventTypeName, opt => opt.MapFrom(src => src.EventType.Name))
                .ForMember(dest => dest.MainPhotoUrl, opt => opt.MapFrom(src =>
                    src.EventPhotos != null && src.EventPhotos.Any()
                        ? src.EventPhotos.OrderBy(p => p.Order).FirstOrDefault()!.Url
                        : string.Empty
                ))
                .ReverseMap();
            CreateMap<Event, EventCreateVM>().ReverseMap();
            CreateMap<Event, EventUpdateVM>().ReverseMap();
            CreateMap<Event, EventDetailsVM>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.Organizer, opt => opt.MapFrom(src => src.Organizer))
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.EventType))
                .ReverseMap();

            //EventPhoto mappings
            CreateMap<EventPhoto, EventPhotoCreateVM>().ReverseMap();
            CreateMap<EventPhoto, EventPhotoUpdateVM>().ReverseMap();
            CreateMap<EventPhoto, EventPhotoDetailsVM>().ReverseMap();




        }
    }
}