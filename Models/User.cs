using Azure.Core.GeoJson;
using Microsoft.AspNetCore.Identity;
using System.Drawing;

namespace SmartAlertAPI.Models
{
    public class User: IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; } = DateTime.Now; 
        public DateTime BirthDate { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Role {  get; set; } = string.Empty;

        public List<Incident> IncidentsReported = [];
    }
}
