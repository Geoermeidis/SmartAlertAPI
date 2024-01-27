using AutoMapper;
using FluentValidation;
using SmartAlertAPI.Models.Dto;
using SmartAlertAPI.Models;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Azure;
using SmartAlertAPI.Repositories;
using SmartAlertAPI.Services;

namespace SmartAlertAPI.Endpoints
{
    public static class AuthEndpoints
    {
        public static void ConfigureAuthEndpoints(this WebApplication app)
        {           
            app.MapPost("/api/login", Login).WithName("Login").Accepts<UserLoginDto>("application/json")
                .Produces<APIResponse>(200).Produces(400);
            app.MapPost("/api/register", Register).WithName("Register").Accepts<UserSignupDto>("application/json")
                .Produces<APIResponse>(200).Produces(400);
            app.MapGet("api/test", () => Results.Ok()).WithName("Test").Produces(200).RequireAuthorization("officer");
        }

        public static IResult Register(IAuthService _authService, [FromBody] UserSignupDto userSignupDto) {
            var response = _authService.Register(userSignupDto);
            return Results.Json(response);
        }

        public static IResult Login(IAuthRepo _authRepo, [FromBody] UserLoginDto userLoginDto) {
            APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
            try
            {
                var token = _authRepo.Login(userLoginDto);
                
                response.Result = token;
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;
                return Results.Ok(response);
            }
            catch (Exception e) {
                response.ErrorMessages.Add("Username or password is incorrect");
                return Results.BadRequest(response);
            }

        }


    }
}
