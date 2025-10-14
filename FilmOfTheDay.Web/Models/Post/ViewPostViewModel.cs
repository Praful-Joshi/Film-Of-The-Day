namespace FilmOfTheDay.Web.Models.Post;

public class ViewPostViewModel
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    public string? MovieUrl { get; set; }

    public string? UserName { get; set; }

    public int UserID { get; set; }

    public DateTime CreatedAt { get; set; }
}