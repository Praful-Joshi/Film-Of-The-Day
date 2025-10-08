using Microsoft.AspNetCore.Mvc;

namespace FilmOfTheDay.Web.Controllers;

public class ProfileController : Controller
{
    // GET: /Profile/Index
    [HttpGet]
    public IActionResult Index() => View();
}

