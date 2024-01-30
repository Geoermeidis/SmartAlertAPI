using SmartAlertAPI.Models;

namespace SmartAlertAPI.Services
{
    public interface INotificationService
    {
        Task SendEventsToUsers(EventRegistered incident); // sends events to co
    }
}
