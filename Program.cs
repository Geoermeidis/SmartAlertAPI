using MagicVilla_CouponAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartAlertAPI.Data;
using SmartAlertAPI.Endpoints;
using SmartAlertAPI.Repositories;
using SmartAlertAPI.Services;
using System.Text;
using FluentValidation;
using SmartAlertAPI.Utils.Validations;
using SmartAlertAPI.Utils.JsonWebToken;
using SmartAlertAPI.Utils.PasswordManager;
using System.Text.Json.Serialization;
using Google.Cloud.Firestore;

var builder = WebApplication.CreateBuilder(args);

Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @".\Data\credentials.json");

builder.Services.AddHttpContextAccessor();

// JSON options
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnStr")));
builder.Services.AddSingleton<FirestoreDb>(s =>
{
    return FirestoreDb.Create("smartalert-2faa1");
});

// Custom Services and repos
builder.Services.AddSingleton<IPasswordManager, PasswordManager>();
builder.Services.AddScoped<IJwtTokenManager, JwtTokenManager>();

builder.Services.AddScoped<IAuthRepo, AuthRepo>();
builder.Services.AddScoped<IIncidentRepo, IncidentRepo>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();

builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IIncidentService, IncidentService>();

// Mapping
builder.Services.AddAutoMapper(typeof(MappingConfig));

// Validations
builder.Services.AddValidatorsFromAssemblyContaining(typeof(AuthRegisterValidation));
builder.Services.AddValidatorsFromAssemblyContaining(typeof(IncidentCreateValidation));

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

// Authentcation
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(0)
        };
    });
// Authorization 
builder.Services.AddAuthorization();

// Authorization policies
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
app.ConfigureIncidentEndpoints();
app.UseHttpsRedirection();

app.Run();