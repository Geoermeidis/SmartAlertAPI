using SmartAlertAPI.Models;
using SmartAlertAPI.Models.DTO;

namespace SmartAlertAPI.Repositories
{
    public interface IIncidentRepo
    {
        ICollection<Incident> GetIncidents();
        ICollection<Incident> GetIncidentByCategory(string category);
        Incident? GetIncidentById(Guid id);
        Incident CreateIncident(IncidentCreateDTO incidentDTO);
        Incident UpdateIncidentStatus(IncidentUpdateDTO incidentDTO, string status);
        // Incident UpdateIncidentCount(IncidentUpd)

    }
}
