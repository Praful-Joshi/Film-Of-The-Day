namespace FilmOfTheDay.Web.Models.Home;

public class HomeFeedViewModel
{
    public List<FeedItemViewModel> FeedItems { get; set; } = new();
}

public class FeedItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? ImageUrl { get; set; }
    public string? UserName { get; set; }
    public int UserID { get; set; }
}