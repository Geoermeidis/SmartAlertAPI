using Azure.Core.GeoJson;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace SmartAlertAPI.Models
{
    public class Incident
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }
        public string Comments { get; set; } = string.Empty;
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public string PhotoURL { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.Now;
        public int TotalSubmissions { get; set; } = 0;
        public IncidentState State { get; set; } = IncidentState.Submitted;
        [ForeignKey("CivilOfficer")]
        public Guid AcceptedById { get; set; }
        public User User { get; set; } = new ();
        public User CivilOfficer { get; set; } = new ();
        public DangerCategory Category { get; set; } = new();
        public List<User> ReportedByUsers { get; set; }
    }

    public enum IncidentState { 
        Rejected = -1,
        Accepted = 1,
        Submitted = 0
    }
}
