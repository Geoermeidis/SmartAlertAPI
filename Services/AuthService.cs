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
            APIResponse response = new();
            try
            {
                var token = _authRepo.Login(userLoginDto);

                response.Result = token;
            }
            catch (Exception ex) when (ex is PasswordDoesntMatchException || ex is UserDoesntExistException )
            {
                response.ErrorMessages.Add("Username or password is incorrect");
            }

            return response;
        }

        public APIResponse Logout()
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

        public APIResponse Register(UserSignupDto userSignupDto)
        {
            APIResponse response = new();
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
    }
}
