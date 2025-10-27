using FilmOfTheDay.Core.Entities;
using FilmOfTheDay.Web.Models.Profile;
using System.Security.Claims;

namespace FilmOfTheDay.Web.Services.Interfaces;
public interface IProfileService
{
    public Task<ProfileViewModel?> GetProfileAsync(int profileUserId, ClaimsPrincipal currentUser);
    public Task<User>? GetLoggedInUserAsync(ClaimsPrincipal currentUser);
}
