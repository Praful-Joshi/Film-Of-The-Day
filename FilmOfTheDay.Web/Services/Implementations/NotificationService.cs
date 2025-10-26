using FilmOfTheDay.Infrastructure.Data;
using FilmOfTheDay.Core.Entities;
using FilmOfTheDay.Web.Services.Interfaces;
using FilmOfTheDay.Web.Models.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace FilmOfTheDay.Web.Services.Implementations;
public class NotificationService : INotificationService
{
    private readonly ApplicationDbContext _dbContext;

    public NotificationService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateNotificationAsync(NotificationItemViewModel notifModel)
    {
        var notification = new Notification
        {
            UserId = notifModel.ReceiverId,
            RelatedEntityId = notifModel.RelatedEntityId,
            Type = notifModel.Type,
            Message = notifModel.Message,
            Link = notifModel.Link,
            IsRead = notifModel.IsRead,
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
                ReceiverId = n.UserId,
                RelatedEntityId = n.RelatedEntityId,
                Type = n.Type,
                Message = n.Message,
                Link = n.Link,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
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
