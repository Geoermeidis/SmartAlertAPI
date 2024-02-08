using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public async Task<ICollection<Incident>> GetIncidents()
        {
            return await _context.Incidents.Include(i => i.User).Include(i => i.Category).ToListAsync();
        }

        public async Task<Incident?> GetIncidentById(Guid id)
        {
            return await _context.Incidents.FirstOrDefaultAsync(c => c.Id == id);
        }
        
        public async Task<ICollection<Incident>> GetIncidentsByCategory(string category)
        {
            
            var cat = await _context.DangerCategories.FirstOrDefaultAsync(c => c.Name.Equals(category));
            if (cat is null) return [];
            var catid = cat.Id;
            return await _context.Incidents.Where(c => c.CategoryId == catid).ToListAsync();
        }

        public async Task<Incident> CreateIncident(IncidentCreateDTORepo incidentDTO)
        {
            Incident incident = _mapper.Map<Incident>(incidentDTO);
            

            await _context.Incidents.AddAsync(incident);
            await _context.SaveChangesAsync();

            return incident;
        }

        public async Task<Incident?> UpdateIncidentStatus(Guid id, string status)
        {
            Incident? dbIncident = await GetIncidentById(id);

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
                await _context.SaveChangesAsync();

                return dbIncident;
            }

            return null;
        }

        public async Task<Incident?> UpdateIncidentSumbissions(Guid id) {
            Incident? dbIncident = await GetIncidentById(id);

            if (dbIncident is not null)
            {
                dbIncident.TotalSubmissions += 1;
                dbIncident.SubmittedAt = DateTime.Now;
                _context.Incidents.Update(dbIncident);
                await _context.SaveChangesAsync();
                return dbIncident;
            }

            return null;
        }

        public async Task<Incident?> GetIncidentWithCategory(Guid id)
        {
            return await _context.Incidents.Include(c => c.Category).FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
