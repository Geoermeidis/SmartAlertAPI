using AutoMapper;
using SmartAlertAPI.Data;
using SmartAlertAPI.Models;
using SmartAlertAPI.Models.DTO;

namespace SmartAlertAPI.Repositories
{
    public class IncidentRepo : IIncidentRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public IncidentRepo(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ICollection<Incident> GetIncidents()
        {
            return _context.Incidents.ToList();
        }

        public Incident? GetIncidentById(Guid id)
        {
            return _context.Incidents.FirstOrDefault(c => c.Id == id);
        }
        
        public ICollection<Incident> GetIncidentByCategory(string category)
        {
            
            var cat = _context.DangerCategories.FirstOrDefault(c => c.Name.Equals(category));
            if (cat is null) return [];
            var catid = cat.Id;
            return _context.Incidents.Where(c => c.CategoryId == catid).ToList();
        }

        public Incident CreateIncident(IncidentCreateDTORepo incidentDTO)
        {
            Incident incident = _mapper.Map<Incident>(incidentDTO);

            _context.Incidents.Add(incident);
            _context.SaveChanges();

            return incident;
        }

        public Incident? UpdateIncidentStatus(Guid id, string status)
        {
            Incident? dbIncident = GetIncidentById(id);

            if (dbIncident is not null)
            {
                IncidentState state = dbIncident.State;

                if (status.ToLower().Equals("accepted"))
                    state = IncidentState.Accepted;
                else if (status.ToLower().Equals("rejected"))
                    state = IncidentState.Rejected;
                else
                    return null;

                dbIncident.State = state;
                
                _context.Incidents.Update(dbIncident);
                _context.SaveChanges();

                return dbIncident;
            }

            return null;
        }

        public Incident? UpdateIncidentSumbissions(Guid id) {
            Incident? dbIncident = GetIncidentById(id);

            if (dbIncident is not null)
            {
                dbIncident.TotalSubmissions += 1;
                _context.Incidents.Update(dbIncident);
                _context.SaveChanges();
                return dbIncident;
            }

            return null;
        }
    }
}
