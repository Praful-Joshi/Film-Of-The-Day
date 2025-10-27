using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmOfTheDay.Core.Entities
{
    public class FilmPost
    {
        [Key]
        public int Id { get; set; }

        public string? Title { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public string MovieUrl { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key to User
        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
    }
}
