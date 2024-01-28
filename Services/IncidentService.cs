using Azure;
using Microsoft.IdentityModel.Tokens;
using SmartAlertAPI.Models;
using SmartAlertAPI.Models.DTO;
using SmartAlertAPI.Repositories;

namespace SmartAlertAPI.Services
{
    public class IncidentService : IIncidentService
    {
        private readonly ICategoryRepo _categoryRepo;
        private readonly IIncidentRepo _incidentRepo;
        private readonly List<string> Categories = ["Volcano", "Earthquake", "Flood"];
        
        public IncidentService(IIncidentRepo incidentRepo, ICategoryRepo categoryRepo)
        {
            _incidentRepo = incidentRepo;
            _categoryRepo = categoryRepo;
        }

        public APIResponse GetIncidentByCategory(string category)
        {
            APIResponse response = new();

            var categories = _categoryRepo.GetDangerCategoriesNames();

            if (category is null) {
                response.ErrorMessages.Add("Category is empty");
                return response;
            }

            if (!categories.Contains(category)) {
                response.ErrorMessages.Add("Category does not exist");
                return response;
            }

            response.Result = _incidentRepo.GetIncidentByCategory(category);

            return response;
        }

        public APIResponse GetIncidentById(Guid id)
        {
            APIResponse response = new();

            if (id.ToString().IsNullOrEmpty())
            {
                response.ErrorMessages.Add("Id is empty");
            }
          
            response.Result = _incidentRepo.GetIncidentById(id);
          
            return response;
        }

        public APIResponse GetIncidents()
        {
            APIResponse response = new() { Result = _incidentRepo.GetIncidents() };

            return response;
        }

        public APIResponse CreateUpdateIncident(IncidentCreateDTO incidentDTO)
        {
            APIResponse response = new();

            return response;

        }

        public APIResponse UpdateIncidentStatus(IncidentUpdateDTO incidentDTO)
        {
            APIResponse response = new();

            return response;
        }
    }
}
