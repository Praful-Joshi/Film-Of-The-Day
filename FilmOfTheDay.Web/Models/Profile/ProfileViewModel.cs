using FilmOfTheDay.Web.Services.Implementations;
using FilmOfTheDay.Web.Services.Interfaces;

namespace FilmOfTheDay.Web.Models.Profile;

public class ProfileViewModel
{
    public string? UserName { get; set; }
    public int UserID { get; set; }
    public string? Email { get; set; }
    public FriendshipState friendshipState { get; set; }
    public int PostCount { get; set; }
    public int FriendsCount { get; set; } // optional
    public List<ProfilePostViewModel> Posts { get; set; } = new();
}

public class ProfilePostViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "";
}