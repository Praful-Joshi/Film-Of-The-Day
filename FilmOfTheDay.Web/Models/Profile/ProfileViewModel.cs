namespace FilmOfTheDay.Web.Models.Profile;

public class ProfileViewModel
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public int PostCount { get; set; }
    public int FriendsCount { get; set; } // optional
    public int SavedCount { get; set; }   // optional
    public List<ProfilePostViewModel> Posts { get; set; } = new();
}

public class ProfilePostViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "https://picsum.photos/300";
}