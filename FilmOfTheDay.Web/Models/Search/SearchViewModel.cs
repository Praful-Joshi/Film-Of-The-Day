namespace FilmOfTheDay.Web.Models.Search;

public class SearchViewModel
{
    public string Query { get; set; } = "";
    public List<SearchedUserViewModel> Users { get; set; } = new();
}

public class SearchedUserViewModel
{
    public int UserID { get; set; }
    public string? UserName { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? Email { get; set; }
}