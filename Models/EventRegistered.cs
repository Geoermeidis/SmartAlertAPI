using Google.Cloud.Firestore;

namespace SmartAlertAPI.Models
{
    public class EventRegistered
    {
        public Guid EventId { get; set; }
        
        public DateTime SubmittedAt { get; set; }
        
        public required double Longitude { get; set; }
        
        public required double Latitude { get; set; }
        
        public required string CategoryName { get; set; }
        
        public required int MaxDistanceNotification { get; set; }
        
        public required string WebsiteURL { get; set; }
    }
}
