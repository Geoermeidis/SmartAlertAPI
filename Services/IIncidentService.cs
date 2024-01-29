using SmartAlertAPI.Models;
using SmartAlertAPI.Models.DTO;

namespace SmartAlertAPI.Services
{
    public interface IIncidentService
    {
        APIResponse GetIncidents();
        APIResponse GetIncidentById(Guid id);
        APIResponse GetIncidentByCategory(string category);
        APIResponse CreateUpdateIncident(IncidentCreateDTO incidentDTO);
        APIResponse UpdateIncidentStatus(Guid id, string status);
    }
}
