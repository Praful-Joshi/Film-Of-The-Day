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
    private readonly ILogger<NotificationController> _logger;

    public NotificationController(
        INotificationService notificationService,
        ILogger<NotificationController> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        try
        {
            var model = await _notificationService.GetUserNotificationsAsync(userId);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving notifications for user {UserId}", userId);
            return View("Error", new NotificationViewModel
            {
                Notifications = new List<NotificationItemViewModel>()
            });
        }
    }

    // --- Private helper for user ID extraction ---
    private bool TryGetUserId(out int userId)
    {
        userId = 0;
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(value, out userId);
    }
}
