using SmartAlertAPI.Models;
using SmartAlertAPI.Models.Dto;

namespace SmartAlertAPI.Services
{
    public interface IAuthService
    {
        Task<APIResponse> Login(UserLoginDto userLoginDto);
        Task<APIResponse> Logout();
        Task<APIResponse> Register(UserSignupDto userSignupDto);
    }
}
