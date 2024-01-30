using System.ComponentModel.DataAnnotations.Schema;

namespace SmartAlertAPI.Models.DTO
{
    public class IncidentCreateDTO
    {
        public string CategoryName { get; set; }
        public string Comments { get; set; }
        public Guid UserId { get; set; }
        public string PhotoURL { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

    }
}
