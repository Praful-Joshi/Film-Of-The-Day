using FilmOfTheDay.Web.Services.Interfaces;
using FilmOfTheDay.Web.Models.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FilmOfTheDay.Web.Controllers;

[Authorize]
public class NotificationController : Controller
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var notifications = await _notificationService.GetUserNotificationsAsync(userId);
        var model = new NotificationViewModel
        {
            Notifications = notifications.Select(n => new NotificationItemViewModel
            {
                Id = n.Id,
                Message = n.Message,
                CreatedAt = n.CreatedAt,
                IsRead = n.IsRead,
                Link = n.Link,
                SenderName = "SenderName"
            }).ToList()
        };
        return View(model);
    }
}