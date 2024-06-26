using SmartAlertAPI.Models;

namespace SmartAlertAPI.Utils.JsonWebToken;

public interface IJwtTokenManager
{
    string CreateToken(User user);
    public string? GetToken();
    public bool IsValid();
    public string GetCurrentUserId();
}