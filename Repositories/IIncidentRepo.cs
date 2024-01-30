using SmartAlertAPI.Models;
using SmartAlertAPI.Models.DTO;

namespace SmartAlertAPI.Repositories
{
    public interface IIncidentRepo
    {
        Task<ICollection<Incident>> GetIncidents();
        Task<Incident?> GetIncidentWithCategory(Guid id);
        Task<ICollection<Incident>> GetIncidentByCategory(string category);
        Task<Incident?> GetIncidentById(Guid id);
        Task<Incident> CreateIncident(IncidentCreateDTORepo incidentDTO);
        Task<Incident?> UpdateIncidentStatus(Guid id, string status);
        Task<Incident?> UpdateIncidentSumbissions(Guid id);
    }
}
