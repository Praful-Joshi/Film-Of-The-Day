using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FilmOfTheDay.Web.Models;
using FilmOfTheDay.Web.Services.Interfaces;
using System.Security.Claims;

namespace FilmOfTheDay.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHomeFeedService _homeFeedService;

    public HomeController(ILogger<HomeController> logger, IHomeFeedService homeFeedService)
    {
        _logger = logger;
        _homeFeedService = homeFeedService;
    }

    public IActionResult Index()
    {
        //Get logged in user id
        var userIdValue = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userIdValue) || !int.TryParse(userIdValue, out var userId))
        {
            return Unauthorized();
        }
        var feedTask = _homeFeedService.GetHomeFeedAsync(userId);
        feedTask.Wait();
        var feed = feedTask.Result;
        return View(feed);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
