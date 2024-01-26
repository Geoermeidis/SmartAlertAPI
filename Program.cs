using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartAlertAPI.Data;
using SmartAlertAPI.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnStr")));

services.AddAuthentication().AddJwtBearer();
services.AddAuthorization();
services.AddAuthorizationBuilder()
  .AddPolicy("officer", policy =>
        policy
            .RequireRole("officer")
            .RequireClaim("scope", "dangers_api"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/handle-errors?view=aspnetcore-8.0
app.UseStatusCodePages(async statusCodeContext
    => await Results.Problem(statusCode: statusCodeContext.HttpContext.Response.StatusCode)
                 .ExecuteAsync(statusCodeContext.HttpContext));


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.Run();