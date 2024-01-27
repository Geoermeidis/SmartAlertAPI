using SmartAlertAPI.Models;
using SmartAlertAPI.Models.Dto;
using SmartAlertAPI.Repositories;
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
            throw new NotImplementedException();
        }

        public APIResponse Logout()
        {
            throw new NotImplementedException();
        }

        public APIResponse Register(UserSignupDto userSignupDto)
        {
            APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

            bool ifUserNameisUnique = _authRepo.IsUserUnique(userSignupDto.Email);
            if (!ifUserNameisUnique)
            {
                response.ErrorMessages.Add("Username already exists");
            }
            // todo add more validations
            try
            {
                _authRepo.Signup(userSignupDto);
                
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                response.ErrorMessages.Add(e.Message);
            }

            return response;
        }
    }
}
