﻿using AutoMapper;
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

            app.MapPost("/api/refresh-token", RefreshToken)
                .WithName("RefreshToken")
                .Accepts<String>("application/json")
                .Produces<APIResponse>(200)
                .Produces(400);
        }

        public async static Task<IResult> Register(IAuthService _authService, [FromBody] UserSignupDto userSignupDto) {

            var response = await _authService.Register(userSignupDto);  // add exception handling for database errors
            if (response.ErrorMessages.IsNullOrEmpty()){
                return Results.Ok(response);
            }
            else {
                return Results.BadRequest(response);
            }

        }

        public async static Task<IResult> Login(IAuthService _authService, [FromBody] UserLoginDto userLoginDto) {
            Console.WriteLine("Login was called");
            var response = await _authService.Login(userLoginDto);
            if (response.ErrorMessages.IsNullOrEmpty())
                return Results.Ok(response);
            else
                return Results.NotFound(response);
        }

        public async static Task<IResult> Logout(IAuthService _authService) {
            var response = await _authService.Logout();
            return Results.Json(response);
        }

        public async static Task<IResult> RefreshToken(IAuthService _authService, [FromBody] string refreshToken) {
            var response = await _authService.RefreshToken(refreshToken);
            if (response.ErrorMessages.IsNullOrEmpty())
                return Results.Ok(response);
            else
                return Results.BadRequest(response);
        }


    }
}
