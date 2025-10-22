namespace FilmOfTheDay.Web.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmOfTheDay.Infrastructure.Data;
using FilmOfTheDay.Web.Models.Search;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class SearchController : Controller
{

    private readonly ApplicationDbContext _dbContext;

    public SearchController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Index(string query)
    {

        Debug.WriteLine("Search query: " + query);
        if (string.IsNullOrWhiteSpace(query))
        {
            // Return empty page or maybe popular users/posts
            return View("EmptySearch");
        }

        //search all users whose username contains the query string
        var searchedUsers = _dbContext.Users
            .AsNoTracking()
            .Where(u => EF.Functions.Like(u.Username, $"%{query}%"))
            .Select(u => new SearchedUserViewModel
            {
                UserID = u.Id,
                UserName = u.Username,
                ProfileImageUrl = "/images/profile-placeholder.png", // Placeholder, replace with actual profile image URL if available
                Email = u.Email
            });

        var model = new SearchViewModel
        {
            Users = searchedUsers.ToList()
        };
        return View(model);
    }
}