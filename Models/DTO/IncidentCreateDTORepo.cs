namespace SmartAlertAPI.Models.DTO
{
    public class IncidentCreateDTORepo
    {
        public string CategoryName { get; set; }
        public string Comments { get; set; }
        public Guid UserId { get; set; }
        public string PhotoURL { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Guid CategoryId { get; set; }
        public DangerCategory Category { get; set; }
    }
}
