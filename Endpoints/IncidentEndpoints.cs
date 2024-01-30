using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmartAlertAPI.Models;
using SmartAlertAPI.Models.Dto;
using SmartAlertAPI.Models.DTO;
using SmartAlertAPI.Services;
using SmartAlertAPI.Utils.Filters;
using System;

namespace SmartAlertAPI.Endpoints
{
    public static class IncidentEndpoints
    {
        public static void ConfigureIncidentEndpoints(this WebApplication app)
        {
            app.MapGet("incidents", GetIncidents).WithName("GetIncidents")
                .Produces<APIResponse>(200)
                .Produces(401)
                .RequireAuthorization();
            
            app.MapGet("incidents/{id}", GetIncidentById).WithName("GetIncidentById")
                .Produces<APIResponse>(200)
                .Produces(400)
                .Produces(401)
                .RequireAuthorization();

            app.MapGet("incidents/category/{danger}", GetIncidentsByDanger)
                .WithName("GetIncidentsByDanger")
                .Produces<APIResponse>(200)
                .Produces(404)
                .Produces(401)
                .RequireAuthorization();

            app.MapPut("incidents/create", CreateIncident).WithName("CreateIncident")
                .Accepts<IncidentCreateDTO>("application/json")
                .AddEndpointFilter<BasicValidationFilter<IncidentCreateDTO>>()
                .Produces<APIResponse>(200)
                .Produces<APIResponse>(400)
                .Produces(401)
                .RequireAuthorization("CivilianRole");

            app.MapPut("incidents/updatestate", UpdateIncident).WithName("UpdateIncident")
                .Produces<APIResponse>(200)
                .Produces(400)
                .Produces(401);
        }

        public async static Task<IResult> GetIncidents(IIncidentService _incidentService) 
        {
            var response = await _incidentService.GetIncidents();
            if (!response.ErrorMessages.IsNullOrEmpty()) {
                return Results.NotFound();
            }
            return Results.Ok(response);
        }
        public async static Task<IResult> GetIncidentById(IIncidentService _incidentService, Guid id)
        {
            var response = await _incidentService.GetIncidentById(id);
            
            if (response.Result == null)
            {
                return Results.BadRequest(response);
            }

            if (!response.ErrorMessages.IsNullOrEmpty()) {
                return Results.BadRequest(response);
            }
            
            return Results.Ok(response);
        }
        public async static Task<IResult> GetIncidentsByDanger(IIncidentService _incidentService, string danger)
        {
            var response = await _incidentService.GetIncidentsByCategory(danger);

            if (response.Result == null)
            {
                return Results.BadRequest(response);
            }

            if (!response.ErrorMessages.IsNullOrEmpty())
            {
                return Results.BadRequest(response);
            }

            return Results.Ok(response);
        }
        public async static Task<IResult> CreateIncident(IIncidentService _incidentService, [FromBody] IncidentCreateDTO incidentDTO)
        {
            // TODO: if new incident is created add its id and some other stuff to firebase so the
            // client can listen to it and retrieve it

            var response = await _incidentService.CreateUpdateIncident(incidentDTO);

            if (response.Result == null)
            {
                return Results.BadRequest(response);
            }

            if (!response.ErrorMessages.IsNullOrEmpty())
            {
                return Results.BadRequest(response);
            }

            return Results.Ok(response);
        }

        public async static Task<IResult> UpdateIncident(IIncidentService _incidentService, Guid id, string status)
        {
            var response = await _incidentService.UpdateIncidentStatus(id, status);

            if (response.Result == null)
            {
                return Results.BadRequest(response);
            }

            if (!response.ErrorMessages.IsNullOrEmpty())
            {
                return Results.BadRequest(response);
            }

            return Results.Ok(response);
        }
    }
}
