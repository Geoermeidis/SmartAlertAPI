using AutoMapper;
using FluentValidation;
using SmartAlertAPI.Models.Dto;
using SmartAlertAPI.Models;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Azure;
using SmartAlertAPI.Repositories;
using SmartAlertAPI.Services;
using SmartAlertAPI.Utils.Exceptions;
using Microsoft.IdentityModel.Tokens;
using SmartAlertAPI.Utils.Filters;

namespace SmartAlertAPI.Endpoints
{
    public static class AuthEndpoints
    {
        public static void ConfigureAuthEndpoints(this WebApplication app)
        {           
            app.MapPost("/api/login", Login)
                .WithName("Login")
                .Accepts<UserLoginDto>("application/json")
                .Produces<APIResponse>(200)
                .Produces(400);
            
            app.MapPost("/api/register", Register)
                .WithName("Register")
                .Accepts<UserSignupDto>("application/json")
                .AddEndpointFilter<BasicValidationFilter<UserSignupDto>>()
                .Produces<APIResponse>(200)
                .Produces(400);
            
            app.MapGet("api/test", () => Results.Ok())
                .WithName("Test")
                .Produces(200)
                .RequireAuthorization("OfficerRole");
        }

        public static IResult Register(IAuthService _authService, [FromBody] UserSignupDto userSignupDto) {

            var response = _authService.Register(userSignupDto);  // add exception handling for database errors
            if (response.IsSuccess){
                return Results.Ok(response);
            }
            else {
                return Results.BadRequest(response);
            }

        }

        public static IResult Login(IAuthService _authService, [FromBody] UserLoginDto userLoginDto) {
            var response = _authService.Login(userLoginDto);
            if (response.IsSuccess)
                return Results.Ok(response);
            else
                return Results.NotFound(response);
        }

        public static IResult Logout(IAuthService _authService) {
            var response = _authService.Logout();
            return Results.Json(response);
        }


    }
}
