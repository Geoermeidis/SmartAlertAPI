using SmartAlertAPI.Models;

namespace SmartAlertAPI.Utils;

public interface IJwtTokenManager
{
    string CreateToken(User user);
    public string? GetToken();
    public bool IsValid();
    public string GetCurrentUserId();
}