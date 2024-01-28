using SmartAlertAPI.Models;
using SmartAlertAPI.Models.Dto;

namespace SmartAlertAPI.Services
{
    public interface IAuthService
    {
        APIResponse Login(UserLoginDto userLoginDto);
        APIResponse Logout();
        APIResponse Register(UserSignupDto userSignupDto);
    }
}
