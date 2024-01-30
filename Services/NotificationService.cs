using AutoMapper;
using Google.Cloud.Firestore;
using SmartAlertAPI.Models;

namespace SmartAlertAPI.Services
{
    public class NotificationService: INotificationService
    {
        private readonly FirestoreDb _firestoreDb;
        private readonly IMapper _mapper;
        private const string _collectionName = "Incidents";

        public NotificationService(FirestoreDb firestoreDb, IMapper mapper)
        {
            _firestoreDb = firestoreDb;
            _mapper = mapper;
        }

        public async Task SendEventsToUsers(EventRegistered incident)
        {
            var collection = _firestoreDb.Collection(_collectionName);
            var shoeDocument = _mapper.Map<EventRegisteredDocument>(incident);
            await collection.AddAsync(shoeDocument);
        }
    }
}
