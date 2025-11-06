using FilmOfTheDay.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmOfTheDay.Web.Controllers;
[Authorize]
public class SearchController : Controller
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string query)
    {
        var model = await _searchService.SearchUsersAsync(query, User);
        return View(model);
    }
}
