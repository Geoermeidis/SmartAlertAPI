using SmartAlertAPI.Data;
using SmartAlertAPI.Models;
using SmartAlertAPI.Models.Dto;
using SmartAlertAPI.Utils;

namespace SmartAlertAPI.Repositories;

public class AuthRepo: IAuthRepo
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IJwtTokenManager _jwtTokenManager;
    private readonly IPasswordManager _passwordManager;
    public AuthRepo(ApplicationDbContext applicationDbContext, IJwtTokenManager jwtTokenManager, IPasswordManager passwordManager)
    {
        _applicationDbContext = applicationDbContext;
        _jwtTokenManager = jwtTokenManager;
        _passwordManager = passwordManager;
    }

    public void Logout()
    {
        var token = _jwtTokenManager.GetToken();
        if(token is null) 
            return;
        _applicationDbContext.TokenBlackList.Add(new TokenBlackList{Token = token});
        _applicationDbContext.SaveChanges();
    }

    public string Login(UserLoginDto userData)
    {
        var user = _applicationDbContext.Users.FirstOrDefault(u => u.Email.Equals(userData.Username) || u.Username.Equals(userData.Username));
        if(user is null)
            throw new Exception("User not found");
        if(!_passwordManager.VerifyPasswordHash(userData.Password, user.PasswordHash, user.PasswordSalt))
            throw new Exception("Invalid password");
        
        return _jwtTokenManager.CreateToken(user);
    }

    public void Signup(UserSignupDto userData)
    {
        if (_applicationDbContext.Users.Any(u => u.Email.Equals(userData.Email)))
            throw new Exception("An user with current email exists already");
        _passwordManager.CreatePasswordHash(userData.Password, out byte[] passwordHash, out byte[] passwordSalt);
        User user = new User {
            FirstName = userData.FirstName,
            LastName = userData.LastName,
            Username = userData.Username,
            PhoneNumber = userData.PhoneNumber,
            Role = "Officer",
            Email = userData.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
        _applicationDbContext.Users.Add(user);
        _applicationDbContext.SaveChanges();
    }

    public bool IsUserUnique(string email)
    {
        return !_applicationDbContext.Users.Any(u => u.Email.Equals(email));
    }

}