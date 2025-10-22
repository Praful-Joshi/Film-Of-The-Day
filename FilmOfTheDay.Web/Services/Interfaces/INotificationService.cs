using FilmOfTheDay.Core.Entities;

namespace FilmOfTheDay.Web.Services.Interfaces;
public interface INotificationService
{
    Task CreateNotificationAsync(int userId, NotificationType type, string message, string? link = null);
    Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId);
    Task MarkAsReadAsync(int notificationId);
}
