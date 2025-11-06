using FilmOfTheDay.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FilmOfTheDay.Web.Controllers;
[Authorize]
public class ConnectionController : Controller
{
    private readonly IConnectionService _connectionService;

    public ConnectionController(IConnectionService connectionService)
    {
        _connectionService = connectionService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int id)
    {
        if (!TryGetUserId(out var loggedInUserId))
            return Unauthorized();

        // If viewing someone else's friends page, you could later add access control
        var model = await _connectionService.GetFriendsPageViewModelAsync(User);
        return View(model);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendRequest(int otherId)
    {
        if (!TryGetUserId(out var senderId))
            return Unauthorized();

        try
        {
            await _connectionService.SendRequestAsync(senderId, otherId);

            if (IsAjaxRequest())
                return Json(new { success = true, message = "Request sent" });

            return RedirectToAction("Index", "Profile", new { id = otherId });
        }
        catch (InvalidOperationException ex)
        {
            if (IsAjaxRequest())
                return Json(new { success = false, message = ex.Message });

            TempData["Error"] = ex.Message;
            return RedirectToAction("Index", "Profile", new { id = otherId });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcceptRequest(int otherId)
    {
        if (!TryGetUserId(out var receiverId))
            return Unauthorized();

        try
        {
            await _connectionService.AcceptRequestAsync(otherId, receiverId);

            if (IsAjaxRequest())
                return Json(new { success = true, message = "Request accepted" });

            return RedirectToAction("Index", "Profile", new { id = otherId });
        }
        catch (InvalidOperationException ex)
        {
            if (IsAjaxRequest())
                return Json(new { success = false, message = ex.Message });

            TempData["Error"] = ex.Message;
            return RedirectToAction("Index", "Profile", new { id = otherId });
        }
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> RemoveFriend(int otherId)
    {
        if (!TryGetUserId(out var userId))
            return Json(new { success = false, message = "Unauthorized" });

        try
        {
            await _connectionService.RemoveFriendAsync(userId, otherId);
            return Json(new { success = true, message = "Friend removed", otherId });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    
    // --- Private helpers ---
    private bool TryGetUserId(out int userId)
    {
        userId = 0;
        var idValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(idValue, out userId);
    }

    private bool IsAjaxRequest()
    {
        return Request.Headers["X-Requested-With"] == "XMLHttpRequest";
    }
}
