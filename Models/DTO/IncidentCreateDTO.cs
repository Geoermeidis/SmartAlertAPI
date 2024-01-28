using System.ComponentModel.DataAnnotations.Schema;

namespace SmartAlertAPI.Models.DTO
{
    public class IncidentCreateDTO
    {
        public string Category { get; set; }
        public string Comments { get; set; }
        public string UserEmail { get; set; }
        public string PhotoURL { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
