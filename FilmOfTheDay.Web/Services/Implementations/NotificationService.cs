using FilmOfTheDay.Infrastructure.Data;
using FilmOfTheDay.Core.Entities;
using FilmOfTheDay.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FilmOfTheDay.Web.Services.Implementations;

public class NotificationService : INotificationService
{
    private readonly ApplicationDbContext _db;

    public NotificationService(ApplicationDbContext db)
    {
        _db = db;
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

        _db.Notifications.Add(notification);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId)
    {
        return await _db.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task MarkAsReadAsync(int notificationId)
    {
        var notif = await _db.Notifications.FindAsync(notificationId);
        if (notif != null)
        {
            notif.IsRead = true;
            await _db.SaveChangesAsync();
        }
    }
}
