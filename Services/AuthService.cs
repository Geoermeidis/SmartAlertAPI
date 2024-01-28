using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SmartAlertAPI.Models;
using SmartAlertAPI.Models.Dto;
using SmartAlertAPI.Repositories;
using SmartAlertAPI.Utils.Exceptions;
using System.Data;
using System.Net;

namespace SmartAlertAPI.Services
{
    public class AuthService: IAuthService
    {
        private readonly IAuthRepo _authRepo;

        public AuthService(IAuthRepo authRepo)
        {
            _authRepo = authRepo;
        }

        public APIResponse Login(UserLoginDto userLoginDto)
        {
            APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
            try
            {
                var token = _authRepo.Login(userLoginDto);

                response.Result = token;
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex) when (ex is PasswordDoesntMatchException || ex is UserDoesntExistException )
            {
                response.ErrorMessages.Add("Username or password is incorrect");
                response.StatusCode = HttpStatusCode.NotFound;
            }

            return response;
        }

        public APIResponse Logout()
        {
            APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
            try
            {
                _authRepo.Logout();
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e){
                response.ErrorMessages = [e.Message];
            }
            return response;
        }

        public APIResponse Register(UserSignupDto userSignupDto)
        {
            APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
            try
            {
                bool UserNameIsUnique = _authRepo.IsUsernameUnique(userSignupDto.Username);
                bool EmailIsUnique = _authRepo.IsEmailUnique(userSignupDto.Email);

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

                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (DBConcurrencyException)
            {
                response.ErrorMessages.Clear();
                response.ErrorMessages.Add("No affected rows");
                response.StatusCode = HttpStatusCode.InternalServerError;
            }
            catch (DbUpdateException)
            {
                response.ErrorMessages.Clear();
                response.ErrorMessages.Add("Problems saving to the database");
                response.StatusCode = HttpStatusCode.InternalServerError;
            }
            catch (Exception) {
                response.ErrorMessages.Clear();
                response.ErrorMessages.Add("Something unexpected happened");
                response.StatusCode = HttpStatusCode.NotFound;
            }
            return response;


        }
    }
}
