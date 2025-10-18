using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmOfTheDay.Infrastructure.Data;
using FilmOfTheDay.Web.Models.Profile;
using Microsoft.AspNetCore.Authorization;

namespace FilmOfTheDay.Web.Controllers
{
    [Authorize] // ensures only logged-in users can access profile
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ProfileController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
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
                Posts = posts
            };

            return View(viewModel);
        }
    }
}
