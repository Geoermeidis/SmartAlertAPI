using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using GeoCoordinatePortable;
using SmartAlertAPI.Models;
using SmartAlertAPI.Models.DTO;
using SmartAlertAPI.Repositories;
using SmartAlertAPI.Utils.Exceptions;
using System.Drawing;

namespace SmartAlertAPI.Services
{
    public class IncidentService : IIncidentService
    {
        private readonly ICategoryRepo _categoryRepo;
        private readonly IIncidentRepo _incidentRepo;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly List<string> Categories = ["Volcano", "Earthquake", "Flood"];
        
        public IncidentService(IIncidentRepo incidentRepo, ICategoryRepo categoryRepo, IMapper mapper, INotificationService notificationService)
        {
            _incidentRepo = incidentRepo;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _notificationService = notificationService;

        }

        public async Task<APIResponse> GetIncidentsByCategory(string category)
        {
            APIResponse response = new();

            var categories = await _categoryRepo.GetDangerCategoriesNames();

            if (category is null) {
                response.ErrorMessages.Add("Category is empty");
                return response;
            }

            if (!categories.Contains(category.ToLower())) {
                response.ErrorMessages.Add("Category does not exist");
                return response;
            }

            var incidents = await _incidentRepo.GetIncidentsByCategory(category);
            incidents.ToList().ForEach(x => x.Category = null);
            response.Result = incidents;

            return response;
        }

        public async Task<APIResponse> GetIncidentById(Guid id)
        {
            APIResponse response = new();

            if (id.ToString().IsNullOrEmpty())
            {
                response.ErrorMessages.Add("Id is empty");
            }
            
            var incident = await _incidentRepo.GetIncidentById(id);

            if (incident is null)
            {
                response.ErrorMessages.Add("Incident doesnt exist");
            }
            else
                response.Result = incident;
            return response;
        }

        public async Task<APIResponse> GetIncidents()
        {
            APIResponse response = new() { Result = await _incidentRepo.GetIncidents() };

            return response;
        }

        public async Task<APIResponse> CreateUpdateIncident(IncidentCreateDTO incidentDTO)
        {
            APIResponse response = new();

            try {
                // get category data by name
                var category = await _categoryRepo.GetCategory(incidentDTO.CategoryName);
             
                // SECOND MAPPING IS USED SO THE CATEGORY ID CAN BE ASSIGNED IN THE SERVICE NOT THE REPO
                IncidentCreateDTORepo incident = _mapper.Map<IncidentCreateDTORepo>(incidentDTO);

                incident.CategoryId = category.Id;
                incident.Category = category;
                
                var incidentDb = await GetIncidentFromDbWithSameConfig(incident);

                if (incidentDb is null)
                {
                    // this means there is no incident reported that matches newer incidents data
                    // so new incident must be created
                    
                    var createdIncident = await _incidentRepo.CreateIncident(incident);
                    response.Result = createdIncident;
                }
                else {
                    // this means there is a similar incident in the database so we just up its counter
                    var updatedIncident = await _incidentRepo.UpdateIncidentSumbissions(incidentDb.Id);
                    response.Result = updatedIncident!;
                }
            }
            catch (Exception ex) when (ex is DbUpdateConcurrencyException || ex is DbUpdateException) {
                response.ErrorMessages.Add("Error accessing the server.");
            }

            return response;
        }

        public async Task<APIResponse> UpdateIncidentStatus(Guid id, string status)
        {
            APIResponse response = new();

            Incident? inc = await _incidentRepo.UpdateIncidentStatus(id, status);
            if (inc is null)
            {
                response.ErrorMessages.Add("Incident doesnt exist or state is not 'Accepted' or 'Rejected'.");
            }
            else {
                if (status.ToLower() == "accepted") { // send object to users/firebase
                    // TODO CREATE MAP
                    Incident? incident = await _incidentRepo.GetIncidentWithCategory(inc.Id);
                    EventRegistered eventRegistered = _mapper.Map<EventRegistered>(incident);
                    await _notificationService.SendEventsToUsers(eventRegistered);
                }
                response.Result = inc;
            }

            return response;
        }

        public async Task<Incident?> GetIncidentFromDbWithSameConfig(IncidentCreateDTORepo incident) { // used to decide wether to actually create the incident or just update its counter
            // find location of reported incident
            GeoCoordinate reportedIncidentLocation = new (incident.Latitude, incident.Longitude);
            // set the maximum datetime, so when comparing with the db incidents,
            // if their datetime is later than this one then 2 incidents are the same
            DateTime minimumDatetimeForSameReport = DateTime.Now.AddMinutes(-incident.Category!.MaxTimeForNewIncident);

            // for every incident in db, if they are in the same category as the reported one,
            // happened later than the min datetime and the reported one happened in the its vicinity
            // THEN AN INCIDENT EXISTS IN DATABASE WITH THE SAME CONFIGS AS THE REPORTED ONE
            // SO THE COUNTER OF THE database incident must go up by 1

            var incidents = await _incidentRepo.GetIncidents();

            Incident? incidentDb = incidents.Where(c => {
                bool isSameCategory = c.CategoryId.Equals(incident.CategoryId);
                bool isReportedIncidentInVicinityOfCurrent = reportedIncidentLocation.GetDistanceTo(new(c.Latitude, c.Longitude)) 
                                                                    < incident.Category.MaxDistanceSubmission;
                bool isCurrentIncidentDateLater = c.SubmittedAt > minimumDatetimeForSameReport;
                
                return isSameCategory && isReportedIncidentInVicinityOfCurrent && isCurrentIncidentDateLater;
            }).FirstOrDefault();

            return incidentDb;
        }
    }
}
