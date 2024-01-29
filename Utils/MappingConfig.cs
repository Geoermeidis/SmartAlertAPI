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
        }
    }
}
