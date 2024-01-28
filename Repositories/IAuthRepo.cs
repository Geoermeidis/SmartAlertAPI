using SmartAlertAPI.Models.Dto;

namespace SmartAlertAPI.Repositories;

public interface IAuthRepo
{
    void Logout();
    string Login(UserLoginDto userData);
    void Signup(UserSignupDto userData);
    bool IsUsernameUnique(string username);
    bool IsEmailUnique(string email); 
}