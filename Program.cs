using MagicVilla_CouponAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.Win32;
using SmartAlertAPI.Data;
using SmartAlertAPI.Endpoints;
using SmartAlertAPI.Models;
using SmartAlertAPI.Repositories;
using SmartAlertAPI.Services;
using System.Text;
using FluentValidation;
using SmartAlertAPI.Utils.Validations;
using SmartAlertAPI.Utils.JsonWebToken;
using SmartAlertAPI.Utils.PasswordManager;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();

// Custom Services
builder.Services.AddSingleton<IPasswordManager, PasswordManager>();
builder.Services.AddScoped<IJwtTokenManager, JwtTokenManager>();
builder.Services.AddScoped<IAuthRepo, AuthRepo>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Mapping
builder.Services.AddAutoMapper(typeof(MappingConfig));

// Validations
builder.Services.AddValidatorsFromAssemblyContaining(typeof(AuthRegisterValidation));

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo Project", Version = "v1" });

    // Add Bearer token support
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
    });
});

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnStr")));

// Authentcation
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("OfficerRole", policy => policy.RequireClaim("role", "Officer").RequireClaim("scope", "alert_api"))
    .AddPolicy("CivilianRole", policy => policy.RequireClaim("role", "Civilian").RequireClaim("scope", "alert_api"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.ConfigureAuthEndpoints();
app.UseHttpsRedirection();

app.Run();