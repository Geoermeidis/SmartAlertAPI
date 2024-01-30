using Google.Cloud.Firestore;

namespace SmartAlertAPI.Models
{
    [FirestoreData]
    public class EventRegisteredDocument
    {
        [FirestoreDocumentId]
        public required  Guid EventId { get; set; }
        [FirestoreProperty]
        public required  DateTime SubmittedAt { get; set; }
        [FirestoreProperty]
        public required  double Longitude { get; set; }
        [FirestoreProperty]
        public required  double Latitude { get; set; }
        [FirestoreProperty]
        public required  string CategoryName { get; set; }
        [FirestoreProperty]
        public required  int TimeForNotification { get; set; }
        [FirestoreProperty]
        public required  int MaxDistanceNotification { get; set; }
        [FirestoreProperty]
        public required  string Description { get; set; }
        [FirestoreProperty]
        public required  string IconURL { get; set; }
        [FirestoreProperty]
        public required  string Instructions { get; set; }
    }
}
