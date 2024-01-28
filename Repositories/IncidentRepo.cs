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
            return _context.Incidents.First(c => c.Id == id);
        }
        
        public ICollection<Incident> GetIncidentByCategory(string category)
        {
            var cat = _context.DangerCategories.FirstOrDefault(c => c.Name.Equals(category));
            if (cat is null) return [];
            return _context.Incidents.Where(c => c.CategoryId.Equals(cat.Id)).ToList();
        }

        public Incident CreateIncident(IncidentCreateDTO incidentDTO)
        {
            Incident incident = _mapper.Map<Incident>(incidentDTO);

            _context.Incidents.Add(incident);
            _context.SaveChanges();

            return incident;
        }

        public Incident UpdateIncidentStatus(IncidentUpdateDTO incidentDTO, string status)
        {
            throw new NotImplementedException();
        }
    }
}
