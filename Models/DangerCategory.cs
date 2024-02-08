namespace SmartAlertAPI.Models
{
    public class DangerCategory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public int MaxDistanceSubmission { get; set; } = 0;
        public int MaxDistanceNotification { get; set; } = 0;
        public int MaxTimeForNewIncident { get; set; } = 0;
        public string Description { get; set; } = string.Empty;
        public string Instructions {get; set; } = string.Empty;
        public string WebsiteURL { get; set; } = string.Empty;

        public List<Incident>? Incidents { get; set; }

    }
}
