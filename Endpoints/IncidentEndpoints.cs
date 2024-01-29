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
                .RequireAuthorization();
            
            app.MapGet("incidents/{id}", GetIncidentById).WithName("GetIncidentById")
                .Produces<APIResponse>(200)
                .Produces(400)
                .RequireAuthorization();

            app.MapGet("incidents/category/{danger}", GetIncidentsByDanger)
                .WithName("GetIncidentsByDanger")
                .Produces<APIResponse>(200)
                .Produces<APIResponse>(400)
                .RequireAuthorization();

            app.MapPut("incidents/create", CreateIncident).WithName("CreateIncident")
                .Accepts<IncidentCreateDTO>("application/json")
                .AddEndpointFilter<BasicValidationFilter<IncidentCreateDTO>>()
                .Produces<APIResponse>(200)
                .Produces<APIResponse>(400)
                .RequireAuthorization("CivilianRole");

            app.MapPut("incidents/updatestate", UpdateIncident).WithName("UpdateIncident")
                .Produces<APIResponse>(200)
                .Produces(400)
                .RequireAuthorization("OfficerRole");
        }

        public static IResult GetIncidents(IIncidentService _incidentService) 
        {
            var response = _incidentService.GetIncidents();
            if (!response.ErrorMessages.IsNullOrEmpty()) {
                return Results.BadRequest();
            }
            return Results.Ok(response);
        }
        public static IResult GetIncidentById(IIncidentService _incidentService, Guid id)
        {
            var response = _incidentService.GetIncidentById(id);
            
            if (response.Result == null)
            {
                return Results.BadRequest(response);
            }

            if (!response.ErrorMessages.IsNullOrEmpty()) {
                return Results.BadRequest(response);
            }
            
            return Results.Ok(response);
        }
        public static IResult GetIncidentsByDanger(IIncidentService _incidentService, string danger)
        {
            var response = _incidentService.GetIncidentByCategory(danger);

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
        public static IResult CreateIncident(IIncidentService _incidentService, [FromBody] IncidentCreateDTO incidentDTO)
        {
            var response = _incidentService.CreateUpdateIncident(incidentDTO);

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

        public static IResult UpdateIncident(IIncidentService _incidentService, Guid id, string status)
        {
            var response = _incidentService.UpdateIncidentStatus(id, status);

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
