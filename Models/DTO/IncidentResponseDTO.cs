using System.ComponentModel.DataAnnotations.Schema;

namespace SmartAlertAPI.Models.DTO
{
    public class IncidentResponseDTO
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public string SubmittedByUsername { get; set; } = string.Empty;
        public string PhotoURL { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string SubmittedAt { get; set; } = string.Empty;
        public int TotalSubmissions { get; set; } = 0;
        public IncidentState State { get; set; } = IncidentState.Submitted;
    }
}
