using SmartAlertAPI.Models;
using SmartAlertAPI.Models.DTO;

namespace SmartAlertAPI.Repositories
{
    public interface IIncidentRepo
    {
        ICollection<Incident> GetIncidents();
        ICollection<Incident> GetIncidentByCategory(string category);
        Incident? GetIncidentById(Guid id);
        Incident CreateIncident(IncidentCreateDTORepo incidentDTO);
        Incident? UpdateIncidentStatus(Guid id, string status);
        Incident? UpdateIncidentSumbissions(Guid id);
    }
}
