using AutoMapper;
using EventManagementSystem.BLL.ViewModels.EventType;
using EventManagementSystem.DAL.Entities;

namespace EventManagementSystem.BLL.Profiles
{
    public class CustomProfile : Profile
    {
        public CustomProfile()
        {
            CreateMap<EventType, EventTypeListVM>().ReverseMap();
            CreateMap<EventType, EventTypeCreateVM>().ReverseMap();
            CreateMap<EventType, EventTypeUpdateVM>().ReverseMap();
            CreateMap<EventType, EventTypeDetailVM>().ReverseMap();
        }
    }
}
