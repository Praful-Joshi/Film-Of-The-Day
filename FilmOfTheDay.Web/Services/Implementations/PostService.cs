using FilmOfTheDay.Core.Entities;
using FilmOfTheDay.Infrastructure.Data;
using FilmOfTheDay.Web.Models.Post;
using FilmOfTheDay.Web.Models.Notification;
using FilmOfTheDay.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace FilmOfTheDay.Web.Services.Implementations;
public class PostService : IPostService
{
    private readonly ApplicationDbContext _db;
    private readonly HttpClient _httpClient;

    private readonly INotificationService _notificationService;
    private readonly string _tmdbApiKey = "e6bd40c5241c0bb8f42b775e601985b1";

    public PostService(ApplicationDbContext db, INotificationService notificationService)
    {
        _db = db;
        _httpClient = new HttpClient();
        _notificationService = notificationService;
    }

    public async Task<(bool Success, string? ErrorMessage)> CreatePostAsync(CreatePostViewModel model, ClaimsPrincipal userPrincipal)
    {
        var userEmail = userPrincipal.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(userEmail))
            return (false, "User email not found.");

        var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == userEmail);
        if (user == null)
            return (false, "User not found.");

        var post = new FilmPost
        {
            Title = model.Title,
            Description = model.Description,
            ImageUrl = model.SelectedMoviePosterUrl ?? "",
            MovieUrl = model.SelectedMovieLink ?? "",
            UserId = user.Id
        };

        _db.FilmPosts.Add(post);
        await _db.SaveChangesAsync();

        await _notificationService.CreateNotificationAsync(new NotificationItemViewModel
        {
            ReceiverId = user.Id,
            RelatedEntityId = post.Id,
            Type = NotificationType.PostCreated,
            Message = $"Check out your new post!",
            Link = $"/Post/ViewPost/{post.Id}"
        });
        
        return (true, null);
    }

    public async Task<CreatePostViewModel> SearchMoviesAsync(CreatePostViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.SearchQuery))
        {
            model.SearchResults = new List<MovieResult>();
            return model;
        }

        var url = $"https://api.themoviedb.org/3/search/movie?api_key={_tmdbApiKey}&query={Uri.EscapeDataString(model.SearchQuery)}";
        var response = await _httpClient.GetStringAsync(url);
        var data = JObject.Parse(response);

        var results = data["results"]?.Select(r => new MovieResult
        {
            Title = r["title"]?.ToString() ?? "",
            PosterUrl = r["poster_path"] != null ? $"https://image.tmdb.org/t/p/w500{r["poster_path"]}" : "",
            MovieUrl = r["id"] != null ? $"https://www.themoviedb.org/movie/{r["id"]}" : ""
        }).ToList();

        model.SearchResults = results ?? new List<MovieResult>();
        return model;
    }

    public async Task<ViewPostViewModel?> GetPostByIdAsync(int id)
    {
        var post = await _db.FilmPosts
            .Include(p => p.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (post == null)
            return null;

        return new ViewPostViewModel
        {
            Title = post.Title,
            Description = post.Description,
            ImageUrl = post.ImageUrl,
            MovieUrl = post.MovieUrl,
            UserName = post.User?.Username ?? "Unknown",
            UserID = post.UserId,
            CreatedAt = post.CreatedAt
        };
    }
}
