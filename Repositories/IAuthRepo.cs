using SmartAlertAPI.Models;
using SmartAlertAPI.Models.Dto;

namespace SmartAlertAPI.Repositories;

public interface IAuthRepo
{
    void Logout();
    Task<string?> Login(UserLoginDto userData);
    void Signup(UserSignupDto userData);
    Task<bool> IsUsernameUnique(string username);
    Task<bool> IsEmailUnique(string email);
    string GenerateRefreshToken();
    Task UpdateRefreshToken(string username, string refreshToken);
    Task<User?> GetByRefreshToken(string token);
}