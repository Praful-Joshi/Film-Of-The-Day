using FilmOfTheDay.Infrastructure.Data;
using FilmOfTheDay.Core.Entities;
using FilmOfTheDay.Web.Services.Interfaces;
using FilmOfTheDay.Web.Models.Notification;
using Microsoft.EntityFrameworkCore;

namespace FilmOfTheDay.Web.Services.Implementations;
public class NotificationService : INotificationService
{
    private readonly ApplicationDbContext _dbContext;

    public NotificationService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateNotificationAsync(int userId, NotificationType type, string message, string? link = null)
    {
        var notification = new Notification
        {
            UserId = userId,
            Type = type,
            Message = message,
            Link = link,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Notifications.Add(notification);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<NotificationViewModel> GetUserNotificationsAsync(int userId)
    {
        var notificationItems = await _dbContext.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new NotificationItemViewModel
            {
                Id = n.Id,
                Message = n.Message,
                CreatedAt = n.CreatedAt,
                IsRead = n.IsRead,
                Link = n.Link,
                SenderName = "", // Placeholder — can map sender info later
                SenderImageUrl = "/images/profile-placeholder.png",
                NotifType = n.Type
            })
            .ToListAsync();

        return new NotificationViewModel
        {
            Notifications = notificationItems
        };
    }


    public async Task MarkAsReadAsync(int notificationId)
    {
        var notif = await _dbContext.Notifications.FindAsync(notificationId);
        if (notif == null)
            return;

        notif.IsRead = true;
        await _dbContext.SaveChangesAsync();
    }
}
