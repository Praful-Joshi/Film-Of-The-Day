using FilmOfTheDay.Web.Models.Profile;
using System.Security.Claims;

namespace FilmOfTheDay.Web.Services.Interfaces;
public interface IProfileService
{
    Task<ProfileViewModel?> GetProfileAsync(int profileUserId, ClaimsPrincipal currentUser);
}
