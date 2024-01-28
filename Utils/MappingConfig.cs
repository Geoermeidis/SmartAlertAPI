using AutoMapper;
using SmartAlertAPI.Models;
using SmartAlertAPI.Models.Dto;

namespace MagicVilla_CouponAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<User, UserSignupDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();
        }
    }
}
