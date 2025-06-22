using AutoMapper;
using EventManagementSystem.BLL.ViewModels.Auth;
using EventManagementSystem.BLL.ViewModels.Event;
using EventManagementSystem.BLL.ViewModels.EventPhoto;
using EventManagementSystem.BLL.ViewModels.EventType;
using EventManagementSystem.BLL.ViewModels.Invitation;
using EventManagementSystem.BLL.ViewModels.Location;
using EventManagementSystem.BLL.ViewModels.LocationPhoto;
using EventManagementSystem.BLL.ViewModels.Organizer;
using EventManagementSystem.BLL.ViewModels.Role;
using EventManagementSystem.BLL.ViewModels.User;
using EventManagementSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System.Data.Common;

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

            //Invitation mappings
            CreateMap<Invitation, InvitationListVM>()
    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)) // Explicit for clarity, but usually mapped by convention
    .ForMember(dest => dest.InvitationCode, opt => opt.MapFrom(src => src.InvitationCode))
    .ForMember(dest => dest.SentAt, opt => opt.MapFrom(src => src.SentAt))
    .ForMember(dest => dest.PersonFullName, opt => opt.MapFrom(src => src.Person.AppUser.Fullname)) // Assuming Person has AppUser, and AppUser has Fullname
    .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Event.Title)) // Assuming Event has Title
    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<InvitationCreateVM, Invitation>()
    // EventId and PersonId are the only properties coming directly from CreateVM
    .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.EventId))
    .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
    // These properties are generated/set by the service layer, not provided by the CreateVM
    .ForMember(dest => dest.SentAt, opt => opt.Ignore())
    .ForMember(dest => dest.InvitationCode, opt => opt.Ignore())
    .ForMember(dest => dest.Status, opt => opt.Ignore())
    // Navigation properties are handled by EF Core when saving based on Ids, or loaded separately.
    .ForMember(dest => dest.Event, opt => opt.Ignore())
    .ForMember(dest => dest.Person, opt => opt.Ignore())
    .ForMember(dest => dest.Participation, opt => opt.Ignore());

            CreateMap<InvitationUpdateVM, Invitation>()
    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status)) // Direct enum to enum mapping

    // Ignore other properties not meant for update or handled elsewhere
    .ForMember(dest => dest.SentAt, opt => opt.Ignore())
    .ForMember(dest => dest.InvitationCode, opt => opt.Ignore())
    .ForMember(dest => dest.EventId, opt => opt.Ignore())
    .ForMember(dest => dest.PersonId, opt => opt.Ignore())
    .ForMember(dest => dest.Event, opt => opt.Ignore())
    .ForMember(dest => dest.Person, opt => opt.Ignore())
    .ForMember(dest => dest.Participation, opt => opt.Ignore());
            CreateMap<Invitation, InvitationDetailsVM>()
    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
    .ForMember(dest => dest.InvitationCode, opt => opt.MapFrom(src => src.InvitationCode))
    .ForMember(dest => dest.SentAt, opt => opt.MapFrom(src => src.SentAt))
    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString())) // Convert enum to string
    .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Event.Title)) // Assuming Event has Title
    .ForMember(dest => dest.EventDescription, opt => opt.MapFrom(src => src.Event.Description)) // Assuming Event has Description
    .ForMember(dest => dest.PersonFullName, opt => opt.MapFrom(src => src.Person.AppUser.Fullname)) // Assuming Person has AppUser and Fullname
    .ForMember(dest => dest.PersonEmail, opt => opt.MapFrom(src => src.Person.AppUser.Email)); // Assuming Per


            //Event Mappings
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

          CreateMap<Organizer, OrganizerListVM>().ReverseMap();
            CreateMap<Organizer, OrganizerCreateVM>().ReverseMap();
            CreateMap<Organizer, OrganizerUpdateVM>().ReverseMap();
            CreateMap<Organizer, OrganizerDetailsVM>()
                .ReverseMap();

            //Role mappings
            CreateMap<IdentityRole, RoleListVM>().ReverseMap();
            CreateMap<IdentityRole, RoleDetailsVM>().ReverseMap();


        }
    }
}