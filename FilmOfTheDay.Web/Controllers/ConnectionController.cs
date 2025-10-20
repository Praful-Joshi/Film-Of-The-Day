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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SendRequest(int receiverId)
    {
        var senderIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(senderIdValue) || !int.TryParse(senderIdValue, out var senderId))
        {
            return Unauthorized();
        }
        
        // Your friend request creation logic here
        _connectionService.SendRequest(senderId, receiverId);

        // Return JSON if it's an AJAX call
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return Json(new { success = true, message = "Request sent" });
        }

        // Fallback for non-AJAX form submission
        return RedirectToAction("Index", "Profile", new { id = receiverId });
    }
}