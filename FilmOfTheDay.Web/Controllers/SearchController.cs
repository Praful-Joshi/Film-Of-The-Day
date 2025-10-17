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
    [HttpGet]
    public IActionResult Index(string query)
    {

        Debug.WriteLine("Search query: " + query);
        if (string.IsNullOrWhiteSpace(query))
        {
            // Return empty page or maybe popular users/posts
            return View("EmptySearch");
        }



        var model = new SearchViewModel();
        return View(model);
    }
}