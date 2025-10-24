using FilmOfTheDay.Web.Models.Post;
using System.Security.Claims;

namespace FilmOfTheDay.Web.Services.Interfaces;
public interface IPostService
{
    Task<(bool Success, string? ErrorMessage)> CreatePostAsync(CreatePostViewModel model, ClaimsPrincipal user);
    Task<CreatePostViewModel> SearchMoviesAsync(CreatePostViewModel model);
    Task<ViewPostViewModel?> GetPostByIdAsync(int id);
}

