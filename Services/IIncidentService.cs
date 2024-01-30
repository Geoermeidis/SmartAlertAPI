using SmartAlertAPI.Models;
using SmartAlertAPI.Models.DTO;

namespace SmartAlertAPI.Services
{
    public interface IIncidentService
    {
        Task<APIResponse> GetIncidents();
        Task<APIResponse> GetIncidentById(Guid id);
        Task<APIResponse> GetIncidentsByCategory(string category);
        Task<APIResponse> CreateUpdateIncident(IncidentCreateDTO incidentDTO);
        Task<APIResponse> UpdateIncidentStatus(Guid id, string status);
    }
}
