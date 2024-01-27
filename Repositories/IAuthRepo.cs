using SmartAlertAPI.Models.Dto;

namespace SmartAlertAPI.Repositories;

public interface IAuthRepo
{
    void Logout();
    string Login(UserLoginDto userData);
    void Signup(UserSignupDto userData);
    bool IsUserUnique(string Email);
}