namespace SmartAlertAPI.Utils.PasswordManager;

public interface IPasswordManager
{
    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
}
