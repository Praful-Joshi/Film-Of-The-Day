using FilmOfTheDay.Web.Services.Interfaces;
using FilmOfTheDay.Web.Models.Connection;
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendRequest(int receiverId)
    {
        if (!TryGetUserId(out var senderId))
            return Unauthorized();

        try
        {
            await _connectionService.SendRequestAsync(senderId, receiverId);

            if (IsAjaxRequest())
                return Json(new { success = true, message = "Request sent" });

            return RedirectToAction("Index", "Profile", new { id = receiverId });
        }
        catch (InvalidOperationException ex)
        {
            if (IsAjaxRequest())
                return Json(new { success = false, message = ex.Message });

            TempData["Error"] = ex.Message;
            return RedirectToAction("Index", "Profile", new { id = receiverId });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcceptRequest([FromBody] AcceptRequestDto dto)
    {
        if (!TryGetUserId(out var receiverId))
            return Unauthorized();

        try
        {
            await _connectionService.AcceptRequestAsync(dto.SenderId, receiverId);

            if (IsAjaxRequest())
                return Json(new { success = true, message = "Request accepted" });

            return RedirectToAction("Index", "Profile", new { id = dto.SenderId });
        }
        catch (InvalidOperationException ex)
        {
            if (IsAjaxRequest())
                return Json(new { success = false, message = ex.Message });

            TempData["Error"] = ex.Message;
            return RedirectToAction("Index", "Profile", new { id = dto.SenderId });
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
