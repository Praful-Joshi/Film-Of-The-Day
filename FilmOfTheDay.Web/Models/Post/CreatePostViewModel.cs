namespace FilmOfTheDay.Web.Models.Post;

public class CreatePostViewModel
{
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "https://picsum.photos/300";
}