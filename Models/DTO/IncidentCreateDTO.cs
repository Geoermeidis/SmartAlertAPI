using System.ComponentModel.DataAnnotations.Schema;

namespace SmartAlertAPI.Models.DTO
{
    public class IncidentCreateDTO
    {
        public required string CategoryName { get; set; }
        public required string Comments { get; set; }
        public required Guid UserId { get; set; }
        public required string PhotoURL { get; set; }
        public required double Latitude { get; set; }
        public required double Longitude { get; set; }
    }
}
