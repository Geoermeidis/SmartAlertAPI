using FluentValidation;
using Google.Apis.Auth.OAuth2.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SmartAlertAPI.Models;
using SmartAlertAPI.Models.Dto;
using SmartAlertAPI.Repositories;
using SmartAlertAPI.Utils.Exceptions;
using SmartAlertAPI.Utils.JsonWebToken;
using System.Data;
using System.Net;

namespace SmartAlertAPI.Services
{
    public class AuthService: IAuthService
    {
        private readonly IAuthRepo _authRepo;
        private readonly IJwtTokenManager _jwtTokenManager;

        public AuthService(IAuthRepo authRepo, IJwtTokenManager jwtTokenManager)
        {
            _authRepo = authRepo;
            _jwtTokenManager = jwtTokenManager;
        }

        public async Task<APIResponse> Login(UserLoginDto userLoginDto)
        {
            APIResponse response = new();
            try
            {
                var token = await _authRepo.Login(userLoginDto);
                var refreshToken = _authRepo.GenerateRefreshToken();
                
                await _authRepo.UpdateRefreshToken(userLoginDto.Username, refreshToken);

                response.Result = new Tokens() {AccessToken = token, RefreshToken = refreshToken };
            }
            catch (Exception ex) when (ex is PasswordDoesntMatchException || ex is UserDoesntExistException )
            {
                response.ErrorMessages.Add("Username or password is incorrect");
            }

            return response;
        }

        public async Task<APIResponse> Logout()
        {
            APIResponse response = new();
            try
            {
               _authRepo.Logout();
            }
            catch (Exception e){
                response.ErrorMessages = [e.Message];
            }
            return response;
        }

        public async Task<APIResponse> Register(UserSignupDto userSignupDto)
        {
            APIResponse response = new();

            try
            {
                bool UserNameIsUnique = await  _authRepo.IsUsernameUnique(userSignupDto.Username);
                bool EmailIsUnique = await _authRepo.IsEmailUnique(userSignupDto.Email);

                if (!UserNameIsUnique)
                {
                    response.ErrorMessages.Add("User name is not unique");
                    return response;
                }
                if (!EmailIsUnique)
                {
                    response.ErrorMessages.Add("Email is not unique");
                    return response;
                }

                _authRepo.Signup(userSignupDto);
            }
            catch (DBConcurrencyException)
            {
                response.ErrorMessages.Clear();
                response.ErrorMessages.Add("No affected rows");
            }
            catch (DbUpdateException)
            {
                response.ErrorMessages.Clear();
                response.ErrorMessages.Add("Problems saving to the database");
            }
            catch (Exception) {
                response.ErrorMessages.Clear();
                response.ErrorMessages.Add("Something unexpected happened");
            }
            return response;


        }

        public async Task<APIResponse> RefreshToken(string refreshToken) {
            // Validate the refresh token
            APIResponse response = new();
            User user = await _authRepo.GetByRefreshToken(refreshToken);

            if (user == null || user.RefreshTokenExpiration < DateTime.Now)
            {
                response.ErrorMessages.Add("Session expired");
                return response;
            }

            // Generate a new access token

            string newAccessToken = _jwtTokenManager.CreateToken(user);
            response.Result = newAccessToken;

            // Return the new access token
            return response;
        }
    }
}
