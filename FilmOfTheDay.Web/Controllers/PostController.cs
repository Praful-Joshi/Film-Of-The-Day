namespace FilmOfTheDay.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FilmOfTheDay.Infrastructure.Data; // your dbcontext namespace
using FilmOfTheDay.Web.Models.Post;

[Authorize]
public class PostController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    public PostController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult CreatePost()
    {
        return View();
    }
}
