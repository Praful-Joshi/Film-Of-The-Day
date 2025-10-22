using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmOfTheDay.Infrastructure.Data;
using FilmOfTheDay.Web.Models.Profile;
using Microsoft.AspNetCore.Authorization;
using FilmOfTheDay.Web.Services.Interfaces;
using System.Security.Claims;
namespace FilmOfTheDay.Web.Controllers
{
    [Authorize] // ensures only logged-in users can access profile
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConnectionService _connectionService;

        public ProfileController(ApplicationDbContext dbContext, IConnectionService connectionService)
        {
            _dbContext = dbContext;
            _connectionService = connectionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            //find the user in the database
            var user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            // get the logged-in user's id from claims and validate it
            var loggedInUserIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedInUserIdStr) || !int.TryParse(loggedInUserIdStr, out var loggedInUserId))
            {
                return Unauthorized();
            }

            // Determine relationship
            var friendshipState = _connectionService.GetFriendshipState(loggedInUserId, id);
            int friendsCount = _connectionService.GetFriendsCount(id);

            //get the user's posts and convert them to list of ProfilePostViewModels
            var posts = await _dbContext.FilmPosts
                .Where(p => p.UserId == user.Id)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new ProfilePostViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync();


            var viewModel = new ProfileViewModel
            {
                UserName = user.Username,
                UserID = user.Id,
                Email = user.Email,
                PostCount = posts.Count,
                FriendsCount = friendsCount,
                Posts = posts,
                friendshipState = friendshipState
            };

            return View(viewModel);
        }
    }
}
