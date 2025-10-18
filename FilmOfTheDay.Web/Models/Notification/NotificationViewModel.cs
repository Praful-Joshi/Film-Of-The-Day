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
}