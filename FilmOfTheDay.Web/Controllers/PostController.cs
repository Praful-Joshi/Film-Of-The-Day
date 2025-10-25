using FilmOfTheDay.Web.Models.Post;
using FilmOfTheDay.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmOfTheDay.Web.Controllers;
[Authorize]
public class PostController : Controller
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreatePostViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePostViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _postService.CreatePostAsync(model, User);
        if (!result.Success)
        {
            ModelState.AddModelError("", result.ErrorMessage ?? "Error creating post.");
            return View(model);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> SearchMovie(CreatePostViewModel model)
    {
        var updatedModel = await _postService.SearchMoviesAsync(model);
        return View("Create", updatedModel);
    }

    [HttpGet]
    public async Task<IActionResult> ViewPost(int id)
    {
        var viewModel = await _postService.GetPostByIdAsync(id);
        if (viewModel == null)
            return NotFound();

        return View(viewModel);
    }
}
