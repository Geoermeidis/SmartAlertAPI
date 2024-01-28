using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmartAlertAPI.Models;
using SmartAlertAPI.Models.DTO;
using SmartAlertAPI.Services;

namespace SmartAlertAPI.Endpoints
{
    public static class IncidentEndpoints
    {
        public static void ConfigureIncidentEndpoints(this WebApplication app) 
        {// TODO AUTHORIZATION
            app.MapGet("incidents", GetIncidents).WithName("GetIncidents")
                .Produces<APIResponse>(200);
            
            app.MapGet("incidents/{id}", GetIncidentById).WithName("GetIncidentById")
                .Produces<APIResponse>(200)
                .Produces<APIResponse>(404);
            
            app.MapGet("incidents/category={danger}", GetIncidentsByDanger).WithName("GetIncidentsByDanger")
                .Produces<APIResponse>(200)
                .Produces(404)
                .Produces(400);
            
            //app.MapPost("incidents/", CreateIncident).WithName("CreateIncident")
            //    .Accepts<IncidentCreateDTO>("application/json")
            //    .Produces<APIResponse>(200);
            
            //app.MapPut("incidents/", UpdateIncident).WithName("UpdateIncident")
            //    .Accepts<IncidentUpdateDTO>("application/json")
            //    .Produces<APIResponse>(200);

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

            if (response.ErrorMessages.IsNullOrEmpty()) {
                return Results.BadRequest(response);
            }

            return Results.Ok(response);
        }
        public static IResult GetIncidentsByDanger(IIncidentService _incidentService, string danger)
        {// add validation for category
            var response = _incidentService.GetIncidentByCategory(danger);

            if (response.ErrorMessages.IsNullOrEmpty())
            {
                return Results.BadRequest(response);
            }

            if (response.Result == null)
            {
                return Results.BadRequest(response);
            }

            return Results.Ok(response);
        }
        //public static IResult CreateIncident(IIncidentService _incidentService, [FromBody] IncidentCreateDTO incidentDTO)
        //{

        //}
        //public static IResult UpdateIncident(IIncidentService _incidentService, [FromBody] IncidentUpdateDTO incidentDTO)
        //{

        //}
    }
}
