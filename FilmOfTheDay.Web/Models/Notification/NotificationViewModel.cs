using FilmOfTheDay.Core.Entities;
namespace FilmOfTheDay.Web.Models.Notification;
public class NotificationViewModel
{
    public List<NotificationItemViewModel> Notifications { get; set; } = new();
}

public class NotificationItemViewModel
{
    public int Id { get; set; }

    // who this notification belongs to
    public int ReceiverId { get; set; }

    // what triggered it (optional foreign key like sender user id)
    public int? RelatedEntityId { get; set; }

    // e.g. "PostLiked", "NewFollower", "CommentReplied", etc.
    public NotificationType? Type { get; set; }

    // readable message, e.g. "John liked your post"
    public string Message { get; set; } = string.Empty;

    // link to take the user somewhere
    public string? Link { get; set; }

    // whether it's been seen
    public bool IsRead { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}