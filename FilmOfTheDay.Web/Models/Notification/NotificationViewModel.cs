using FilmOfTheDay.Core.Entities;
namespace FilmOfTheDay.Web.Models.Notification;
public class NotificationViewModel
{
    public List<NotificationItemViewModel> Notifications { get; set; } = new();
}

public class NotificationItemViewModel
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    public string? SenderImageUrl { get; set; }
    public string? Link { get; set; }
    public string? SenderName { get; set; }
    public NotificationType? NotifType { get; set; }
}