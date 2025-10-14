using FilmOfTheDay.Core.Entities;
using FilmOfTheDay.Infrastructure.Data;
using FilmOfTheDay.Web.Models.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class PostController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly HttpClient _httpClient;
    private readonly string _tmdbApiKey = "e6bd40c5241c0bb8f42b775e601985b1";

    public PostController(ApplicationDbContext context)
    {
        _dbContext = context;
        _httpClient = new HttpClient();
    }

    // GET: /Post/Create
    public IActionResult Create()
    {
        return View(new CreatePostViewModel());
    }

    // POST: Save Post
    [HttpPost]
    public async Task<IActionResult> Create(CreatePostViewModel model)
    {
        //get the logged in user's email from the claims
        var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

        //find the user in the database
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == userEmail);
            
        if (user == null)
            return NotFound();

        var post = new FilmPost
        {
            Title = model.Title,
            Description = model.Description,
            ImageUrl = model.SelectedMoviePosterUrl ?? "",
            MovieUrl = model.SelectedMovieLink ?? "",
            UserId = user.Id
        };

        _dbContext.FilmPosts.Add(post);
        await _dbContext.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }

    // POST: Search Movie
    [HttpPost]
    public async Task<IActionResult> SearchMovie(CreatePostViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.SearchQuery))
            return View("Create", model);

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

        return View("Create", model);
    }
}
