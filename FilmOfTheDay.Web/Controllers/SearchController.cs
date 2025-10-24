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
        if (string.IsNullOrWhiteSpace(query))
            return View("EmptySearch");

        var model = await _searchService.SearchUsersAsync(query);
        return View(model);
    }
}
