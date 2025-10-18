using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmOfTheDay.Infrastructure.Data;
using FilmOfTheDay.Web.Models.Notification;
using Microsoft.AspNetCore.Authorization;

namespace FilmOfTheDay.Web.Controllers;

[Authorize]
public class NotificationController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public NotificationController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var model = new NotificationViewModel
        {
            Notifications = new List<NotificationItemViewModel>()
        };

        return View(model);
    }
}