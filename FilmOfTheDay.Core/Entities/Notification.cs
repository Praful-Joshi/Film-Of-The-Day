using System.ComponentModel.DataAnnotations;
namespace FilmOfTheDay.Core.Entities;

public enum NotificationType
{
    PostLiked,
    NewFollower,
    CommentReplied,
    // add more as needed
}

public class Notification
{
    public int Id { get; set; }

    // who this notification belongs to
    public int UserId { get; set; }

    // what triggered it (optional foreign key)
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
