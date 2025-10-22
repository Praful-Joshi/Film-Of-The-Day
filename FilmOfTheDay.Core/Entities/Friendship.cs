using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmOfTheDay.Core.Entities
{
    public class Friendship
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SenderId { get; set; }

        [ForeignKey("SenderId")]
        public User User { get; set; } = null!;

        [Required]
        public int ReceiverId { get; set; }

        [ForeignKey("ReceiverId")]
        public User Friend { get; set; } = null!;

        [Required]
        public FriendshipStatus Status { get; set; } = FriendshipStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum FriendshipStatus
    {
        Pending,
        Accepted,
        Rejected
    }
}
