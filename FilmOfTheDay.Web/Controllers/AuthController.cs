using FilmOfTheDay.Web.Models.Auth;
using FilmOfTheDay.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FilmOfTheDay.Web.Controllers;
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (_authService.IsAuthenticated(User))
            return RedirectToAction("Index", "Home");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _authService.RegisterAsync(model);

        if (!result.Success)
        {
            ModelState.AddModelError("", result.ErrorMessage!);
            return View(model);
        }

        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (_authService.IsAuthenticated(User))
            return RedirectToAction("Index", "Home");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _authService.LoginAsync(model);

        if (!result.Success)
        {
            ModelState.AddModelError("", result.ErrorMessage!);
            return View(model);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return RedirectToAction("Login", "Auth");
    }
}
