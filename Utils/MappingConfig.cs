using AutoMapper;
using SmartAlertAPI.Models;
using SmartAlertAPI.Models.Dto;
using SmartAlertAPI.Models.DTO;

namespace MagicVilla_CouponAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<User, UserSignupDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();
            CreateMap<Incident, IncidentCreateDTORepo>().ReverseMap();
            CreateMap<IncidentCreateDTO, IncidentCreateDTORepo>().ReverseMap();
            CreateMap<EventRegistered, EventRegisteredDocument>().ReverseMap();
            CreateMap<Incident, EventRegistered>()
                .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SubmittedAt, opt => opt.MapFrom(src => src.SubmittedAt.ToUniversalTime()))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category!.Name))
                .ForMember(dest => dest.TimeForNotification, opt => opt.MapFrom(src => src.Category!.TimeForNotification))
                .ForMember(dest => dest.MaxDistanceNotification, opt => opt.MapFrom(src => src.Category!.MaxDistanceNotification))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Category!.Description))
                .ForMember(dest => dest.Instructions, opt => opt.MapFrom(src => src.Category!.Instructions));
            CreateMap<Incident, IncidentResponseDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.SubmittedByUsername, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.PhotoURL, opt => opt.MapFrom(src => src.PhotoURL))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude))
                .ForMember(dest => dest.SubmittedAt, opt => opt.MapFrom(src => src.SubmittedAt.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.TotalSubmissions, opt => opt.MapFrom(src => src.TotalSubmissions))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State));



        }
    }
}
