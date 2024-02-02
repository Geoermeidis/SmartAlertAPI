using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartAlertAPI.Data;
using SmartAlertAPI.Models;
using SmartAlertAPI.Models.Dto;
using SmartAlertAPI.Utils.Exceptions;
using SmartAlertAPI.Utils.JsonWebToken;
using SmartAlertAPI.Utils.PasswordManager;
using System.Data;
using System.Security.Cryptography;

namespace SmartAlertAPI.Repositories;

public class AuthRepo: IAuthRepo
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IJwtTokenManager _jwtTokenManager;
    private readonly IPasswordManager _passwordManager;
    private readonly IMapper _mapper;

    public AuthRepo(ApplicationDbContext applicationDbContext, IMapper mapper, IJwtTokenManager jwtTokenManager, IPasswordManager passwordManager)
    {
        _applicationDbContext = applicationDbContext;
        _jwtTokenManager = jwtTokenManager;
        _passwordManager = passwordManager;
        _mapper = mapper;
    }

    public async void Logout()
    {
        var token = _jwtTokenManager.GetToken() ?? throw new Exception("You are not signed in");
        await _applicationDbContext.TokenBlackList.AddAsync(new TokenBlackList{Token = token});
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<string?> Login(UserLoginDto userData)
    {
        var user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Email.Equals(userData.Username) || u.Username.Equals(userData.Username));

        if (user is null)
            throw new UserDoesntExistException("User not found");
        if(!_passwordManager.VerifyPasswordHash(userData.Password, user.PasswordHash, user.PasswordSalt))
            throw new PasswordDoesntMatchException("Invalid password");
        
        return _jwtTokenManager.CreateToken(user);
    }

    public void Signup(UserSignupDto userData)
    {
        _passwordManager.CreatePasswordHash(userData.Password, out byte[] passwordHash, out byte[] passwordSalt);

        User user = _mapper.Map<User>(userData);

        user.Role = "Civilian";
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;


        _applicationDbContext.Users.Add(user);
        _applicationDbContext.SaveChanges();
    }

    public async Task<bool> IsUsernameUnique(string username)
    {
        return await _applicationDbContext.Users.AnyAsync(u => u.Username.Equals(username)) == false;
    }

    public async Task<bool> IsEmailUnique(string email){
        return await _applicationDbContext.Users.AnyAsync(u => u.Email.Equals(email)) == false;
    }

    public string GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[128];
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    public async Task UpdateRefreshToken(string username, string refreshToken)
    {
        var user = await _applicationDbContext.Users.FirstOrDefaultAsync(c => c.Username.Equals(username));
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiration = DateTime.Now.AddDays(7);

        _applicationDbContext.Users.Update(user);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<User?> GetByRefreshToken(string token) {
        return await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.RefreshToken == token);
    }

}