using Google.Cloud.Firestore;

namespace SmartAlertAPI.Models
{
    public class EventRegistered
    {
        public Guid EventId { get; set; }
        
        public DateTime SubmittedAt { get; set; }
        
        public double Longitude { get; set; }
        
        public double Latitude { get; set; }
        
        public string CategoryName { get; set; }
        
        public int TimeForNotification { get; set; } = 0;
        
        public int MaxDistanceNotification { get; set; } = 0;
        
        public string Description { get; set; } = string.Empty;
        
        public string IconURL { get; set; } = string.Empty;
        
        public string Instructions { get; set; } = string.Empty;
    }
}
