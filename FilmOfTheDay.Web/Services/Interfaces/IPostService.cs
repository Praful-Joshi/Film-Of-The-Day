using FilmOfTheDay.Web.Models.Post;
using System.Security.Claims;

namespace FilmOfTheDay.Web.Services.Interfaces;
public interface IPostService
{
    public Task<(bool Success, string? ErrorMessage)> CreatePostAsync(CreatePostViewModel model, ClaimsPrincipal user);
    public Task<CreatePostViewModel> SearchMoviesAsync(CreatePostViewModel model);
    public Task<ViewPostViewModel?> GetPostByIdAsync(int id);
}

