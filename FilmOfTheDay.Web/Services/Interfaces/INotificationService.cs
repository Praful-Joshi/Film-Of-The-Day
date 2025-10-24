using FilmOfTheDay.Core.Entities;

namespace FilmOfTheDay.Web.Services.Interfaces;
public interface INotificationService
{
    public Task CreateNotificationAsync(int userId, NotificationType type, string message, string? link = null);
    public Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId);
    public Task MarkAsReadAsync(int notificationId);
}
