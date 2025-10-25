using FilmOfTheDay.Core.Entities;
using FilmOfTheDay.Web.Models.Notification;

namespace FilmOfTheDay.Web.Services.Interfaces;
public interface INotificationService
{
    public Task CreateNotificationAsync(int userId, NotificationType type, string message, string? link = null);
    public Task<NotificationViewModel> GetUserNotificationsAsync(int userId);
    public Task MarkAsReadAsync(int notificationId);
}
